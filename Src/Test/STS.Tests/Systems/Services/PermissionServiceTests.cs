using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.ViewModels;
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
        public async void Get_By_RoleId_Should_Return_Collection_Permissions()
        {
            //Arrange
            var roleId = 1;
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(roleId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<PermissionViewModel>>();
            result.Should().HaveCount(data.Count());
        }

        [Fact]
        public async void Get_By_RoleId_And_PermissionId_Should_Return_Permission()
        {
            //Arrange
            long roleId = 1;
            long permissionId = 1;
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(roleId, permissionId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PermissionViewModel>();
            result.Id.Should().Be(permissionId);
            result.RoleId.Should().Be(roleId);
        }

        [Fact]
        public async void Get_By_RoleId_And_Invalid_PermissionId_Should_Return_Null()
        {
            //Arrange
            long roleId = 1;
            long permissionId = -1;
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(roleId, permissionId);

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

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(title);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsTitleDuplicate_By_Duplicate_Title_Shold_Return_True()
        {
            //Arrange
            string title = "Permission_1";

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(title);

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

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(permissionId, title);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsTitleDuplicate_Exact_Record_Shold_Return_False()
        {
            //Arrange
            long permissionId = 1;
            string title = "Permission_1";

            _mockPermissionSet.IqueryableRegisteration<Permission>(PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(permissionId, title);

            //Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
