using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.RoleModels.ViewModels;
using STS.Services.Impls;
using STS.Tests.Helpers;
using STS.Tests.MockDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace STS.Tests.Systems.Services
{
    public class RoleServiceTests
    {
        private Mock<DbSet<Role>> _mockRoleSet;
        private Mock<STSDbContext> _mockContext;

        public RoleServiceTests()
        {
            _mockRoleSet = new Mock<DbSet<Role>>();
            _mockContext = new Mock<STSDbContext>();
        }

        #region Add

        [Fact]
        public async void Add_Save_A_Role_Via_Context()
        {
            //Arrange
            var addFormModel = RoleMockDatas.AddFormModel();
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.AddAsync(addFormModel);

            //Assert
            _mockRoleSet.Verify(m => m.AddAsync(It.IsAny<Role>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Get

        [Fact]
        public async void Get_By_ApplicationId_Return_Collection_Role()
        {
            //Arrange
            var applicationId = 1;
            var data = RoleMockDatas.RoleCollectionEntityModels().AsQueryable();

            _mockRoleSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.GetAsync(applicationId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<RoleViewModel>>();
            result.Should().HaveCount(data.Count());
        }

        [Fact]
        public async void Get_By_ApplicationId_And_RoleId_Should_Return_Role()
        {
            //Arrange
            long roleId = 1;
            long ApplicationId = 1;
            var data = RoleMockDatas.RoleCollectionEntityModels().AsQueryable();

            _mockRoleSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.GetAsync(ApplicationId, roleId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RoleViewModel>();
            result.Id.Should().Be(roleId);
            result.ApplicationId.Should().Be(ApplicationId);
        }

        [Fact]
        public async void Get_By_ApplicationId_And_Invalid_RoleId_Should_Return_Null()
        {
            //Arrange
            long roleId = 1;
            long applicationId = -1;
            var data = RoleMockDatas.RoleCollectionEntityModels().AsQueryable();

            _mockRoleSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.GetAsync(applicationId, roleId);

            //Assert
            result.Should().BeNull();
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_Save_A_Role_Via_Context()
        {
            //Arrange
            var updateFormModel = RoleMockDatas.UpdateFormModel();
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);
            _mockRoleSet.Setup(m => m.FindAsync(updateFormModel.Id)).ReturnsAsync(RoleMockDatas.EntityModel());

            //Act
            var sut = new RoleService(_mockContext.Object);
            await sut.UpdateAsync(updateFormModel);

            //Assert
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion


        #region Delete

        [Fact]
        public async void Delete_Save_A_Role_Via_Context()
        {
            //Arrange
            long roleId = 1;
            var entity = RoleMockDatas.EntityModel();

            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);
            _mockRoleSet.Setup(m => m.FindAsync(roleId)).ReturnsAsync(entity);

            //Act
            var sut = new RoleService(_mockContext.Object);
            await sut.DeleteAsync(roleId);

            //Assert
            _mockRoleSet.Verify(m => m.Remove(entity), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region IsRoleValid

        [Fact]
        public async void IsRoleValid_By_Valid_RoleId_Shold_Return_True()
        {
            //Arrange
            long roleId = 1;

            _mockRoleSet.IqueryableRegisteration<Role>(RoleMockDatas.RoleCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.IsRoleValidAsync(roleId);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsRoleValid_By_Invalid_RoleId_Shold_Return_False()
        {
            //Arrange
            long roleId = -1;

            _mockRoleSet.IqueryableRegisteration<Role>(RoleMockDatas.RoleCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.IsRoleValidAsync(roleId);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region IsTitleDuplicate

        [Fact]
        public async void IsCaptionDuplicate_By_NonDuplicate_Caption_Shold_Return_False()
        {
            //Arrange
            string caption = "Role_Unique";

            _mockRoleSet.IqueryableRegisteration<Role>(RoleMockDatas.RoleCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.IsCaptionDuplicateAsync(caption);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsCaptionDuplicate_By_Duplicate_Caption_Shold_Return_True()
        {
            //Arrange
            string title = "Role_1";

            _mockRoleSet.IqueryableRegisteration<Role>(RoleMockDatas.RoleCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.IsCaptionDuplicateAsync(title);

            //Assert
            result.Should().BeTrue();
        }

        #endregion

        #region IsTitleDuplicate

        [Fact]
        public async void IsCaptionDuplicate_By_RoleId_And_NonDuplicate_Caption_Shold_Return_False()
        {
            //Arrange
            long roleId = 1;
            string title = "Role_Unique";

            _mockRoleSet.IqueryableRegisteration<Role>(RoleMockDatas.RoleCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.IsCaptionDuplicateAsync(roleId, title);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsCaptionDuplicate_By_RoleId_And_Duplicate_Caption_Shold_Return_True()
        {
            //Arrange
            long roleId = 2;
            string title = "Role_1";

            _mockRoleSet.IqueryableRegisteration<Role>(RoleMockDatas.RoleCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.IsCaptionDuplicateAsync(roleId, title);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsCaptionDuplicate_Exact_Record_Shold_Return_False()
        {
            //Arrange
            long roleId = 1;
            string title = "Role_1";

            _mockRoleSet.IqueryableRegisteration<Role>(RoleMockDatas.RoleCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Roles).Returns(_mockRoleSet.Object);

            //Act
            var sut = new RoleService(_mockContext.Object);
            var result = await sut.IsCaptionDuplicateAsync(roleId, title);

            //Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
