using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.Services.Impls;
using STS.Tests.Helpers;
using STS.Tests.MockDatas;
using System.Collections.Generic;
using System.Linq;
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

        #region

        [Fact]
        public async void Get_By_RoleId_Should_Return_Collection_Permissions()
        {
            //Arrange
            var roleId = 1;
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Permission>(data.Provider));
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(roleId);

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(data.Count());
        }


        [Fact]
        public async void Get_By_RoleId_And_PermissionId_Should_Return_Permission()
        {
            //Arrange
            long roleId = 1;
            long permissionId = 1;
            var data = PermissionMockDatas.PermissionCollectionEntityModels().AsQueryable();

            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Permission>(data.Provider));
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(roleId, permissionId);

            //Assert
            result.Should().NotBeNull();
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

            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Permission>(data.Provider));
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.Expression).Returns(data.Expression);
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _mockPermissionSet.As<IQueryable<Permission>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.Permissions).Returns(_mockPermissionSet.Object);

            //Act
            var sut = new PermissionService(_mockContext.Object);
            var result = await sut.GetAsync(roleId, permissionId);

            //Assert
            result.Should().BeNull();
        }

        #endregion
    }
}
