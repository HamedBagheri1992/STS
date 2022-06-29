using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using STS.Common.BaseModels;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Services.Impls;
using STS.Tests.Helpers;
using STS.Tests.MockDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Xunit;

namespace STS.Tests.Systems.Services
{
    public class PermissionServiceTests
    {
        private Mock<DbSet<Permission>> _mockPermissionSet;
        private Mock<STSDbContext> _mockContext;

        public PermissionServiceTests()
        {
            _mockPermissionSet = new Mock<DbSet<Permission>>();
            _mockContext = new Mock<STSDbContext>();
        }

        #region Add

        [Fact]
        public async void Add_Save_A_Permission_Via_Context()
        {
            //Arrange
            var addFormModel = PermissionMockDatas.AddFormModel();
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.AddAsync(addFormModel);

            //Assert
            _mockPermissionSet.Verify(m => m.AddAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Get

        [Fact]
        public async void Get_By_ApplicationId_Should_Return_Collection_Permissions()
        {
            //Arrange
            var applicationId = 1;
            var pagination = new PaginationParam { PageNumber = 1, PageSize = 10 };
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(applicationId, pagination);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PaginatedResult<PermissionViewModel>>();
            result.Items.Should().HaveCount(data.Count());
        }

        [Fact]
        public async void Get_By_ApplicationId_And_PermissionId_Should_Return_Permission()
        {
            //Arrange
            long applicationId = 1;
            long permissionId = 1;
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(applicationId, permissionId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PermissionViewModel>();
            result.Id.Should().Be(permissionId);
            result.ApplicationId.Should().Be(applicationId);
        }

        [Fact]
        public async void Get_By_ApplicationId_And_Invalid_PermissionId_Should_Return_Null()
        {
            //Arrange
            long applicationId = 1;
            long permissionId = -1;
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(applicationId, permissionId);

            //Assert
            result.Should().BeNull();
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_Save_A_Permission_Via_Context()
        {
            //Arrange
            var updateFormModel = PermissionMockDatas.UpdateFormModel();
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);
            _mockPermissionSet.Setup(m => m.FindAsync(updateFormModel.Id)).ReturnsAsync(PermissionMockDatas.EntityModel());

            //Act
            var sut = new PermissionService(_mockContext.Object);
            await sut.UpdateAsync(updateFormModel);

            //Assert
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region UpdatePermissionCategory

        [Fact]
        public async void UpdatePermissionCategory_Should_Call_SaveChanges()
        {
            //Arrange
            var updateFormModel = new UpdatePermissionCategoryFormModel { CategoryId = 1, PermissionIds = new List<long> { 1, 2, 3 } };
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            await sut.UpdatePermissionCategoryAsync(updateFormModel);


            //Assert            
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void UpdatePermissionCategory_By_Invalid_Permission_Collection_Should_Return_Exception()
        {
            //Arrange
            var updateFormModel = new UpdatePermissionCategoryFormModel { CategoryId = 1, PermissionIds = new List<long> { 1, 2, 4 } };
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var ex = await Assert.ThrowsAsync<STSException>(async () => await sut.UpdatePermissionCategoryAsync(updateFormModel));

            //Assert            
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Some of the Permissions are invalid");
        }

        [Fact]
        public async void UpdatePermissionCategory_Which_Permission_By_Diffrent_ApplicationId_Should_Return_Exception()
        {
            //Arrange
            var updateFormModel = new UpdatePermissionCategoryFormModel { CategoryId = 1, PermissionIds = new List<long> { 1, 2, 3 } };

            var parameters = PermissionMockDatas.PermissionCollectionEntityModels();
            parameters[0].ApplicationId = 2;
            var data = parameters.AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var ex = await Assert.ThrowsAsync<STSException>(async () => await sut.UpdatePermissionCategoryAsync(updateFormModel));

            //Assert            
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Permission are not in an Application");
        }

        #endregion

        #region Delete

        [Fact]
        public async void Delete_Save_A_Permission_Via_Context()
        {
            //Arrange
            long permissionId = 1;
            var entity = PermissionMockDatas.EntityModel();

            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);
            _mockPermissionSet.Setup(m => m.FindAsync(permissionId)).ReturnsAsync(entity);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            await sut.DeleteAsync(permissionId);

            //Assert
            _mockPermissionSet.Verify(m => m.Remove(entity), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region IsPermissionValid

        [Fact]
        public async void IsPermissionValid_By_Valid_PermissionId_Shold_Return_True()
        {
            //Arrange
            long permissionId = 1;

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsPermissionValidAsync(permissionId);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsPermissionValid_By_Invalid_PermissionId_Shold_Return_False()
        {
            //Arrange
            long permissionId = -1;

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsPermissionValidAsync(permissionId);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region IsTitleDuplicate

        [Fact]
        public async void IsTitleDuplicate_By_NonDuplicate_Title_Shold_Return_False()
        {
            //Arrange
            string title = "Permission_Unique";
            long applicationId = 1;

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(applicationId, title);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsTitleDuplicate_By_Duplicate_Title_Shold_Return_True()
        {
            //Arrange
            string title = "Permission_1";
            long applicationId = 1;

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(applicationId, title);

            //Assert
            result.Should().BeTrue();
        }

        #endregion

        #region IsTitleDuplicate

        [Fact]
        public async void IsTitleDuplicate_By_PermissionId_And_NonDuplicate_Title_Shold_Return_False()
        {
            //Arrange
            long permissionId = 1;
            string title = "Permission_Unique";

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(permissionId, title);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsTitleDuplicate_By_PermissionId_And_Duplicate_Title_Shold_Return_True()
        {
            //Arrange
            long permissionId = 2;
            string title = "Permission_1";
            long applicationId = 1;

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(applicationId, permissionId, title);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsTitleDuplicate_Exact_Record_Shold_Return_False()
        {
            //Arrange
            long permissionId = 1;
            string title = "Permission_1";
            long applicationId = 1;

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(applicationId, permissionId, title);

            //Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
