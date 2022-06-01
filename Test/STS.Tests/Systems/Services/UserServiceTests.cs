using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Services.Impls;
using STS.Tests.Helpers;
using STS.Tests.MockDatas;
using System.Linq;
using System.Threading;
using Xunit;

namespace STS.Tests.Systems.Services
{
    public class UserServiceTests
    {
        private Mock<DbSet<User>> _mockUserSet;
        private Mock<STSDbContext> _mockContext;

        public UserServiceTests()
        {
            _mockUserSet = new Mock<DbSet<User>>();
            _mockContext = new Mock<STSDbContext>();
        }

        #region Get

        [Fact]
        public async void Get_By_Pagination_Should_Return_PagedList_Collection_User()
        {
            //Arrange
            var pagination = new PaginationParam { PageNumber = 1, PageSize = 10 };
            var data = UserMockDatas.UserCollectionEntityModels().AsQueryable();

            _mockUserSet.IqueryableRegisteration(data);
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.GetAsync(pagination);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PaginatedResult<UserViewModel>>();
            result.Items.Should().HaveCount(data.Count());
        }

        [Fact]
        public async void Get_By_Id_Should_Return_User()
        {
            //Arrange            
            long id = 1;
            var data = UserMockDatas.UserCollectionEntityModels().AsQueryable();

            _mockUserSet.IqueryableRegisteration(data);
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.GetAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserViewModel>();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async void Get_By_Invalid_Id_Should_Return_Null()
        {
            //Arrange            
            long id = -1;
            var data = UserMockDatas.UserCollectionEntityModels().AsQueryable();

            _mockUserSet.IqueryableRegisteration(data);
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.GetAsync(id);

            //Assert
            result.Should().BeNull();
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_Save_A_User_Via_Context()
        {
            //Arrange
            var updateFormModel = UserMockDatas.UpdateFormModel();
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);
            _mockUserSet.Setup(m => m.FindAsync(updateFormModel.Id)).ReturnsAsync(UserMockDatas.EntityModel());

            //Act
            var sut = new UserService(_mockContext.Object);
            await sut.UpdateAsync(updateFormModel);

            //Assert
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Delete

        [Fact]
        public async void Delete_Save_A_User_Via_Context()
        {
            //Arrange
            long id = 1;
            var entity = UserMockDatas.EntityModel();

            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);
            _mockUserSet.Setup(m => m.FindAsync(id)).ReturnsAsync(entity);

            //Act
            var sut = new UserService(_mockContext.Object);
            await sut.DeleteAsync(id);

            //Assert
            _mockUserSet.Verify(m => m.Remove(entity), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region IsExist

        [Fact]
        public async void IsExist_By_Valid_Id_Shold_Return_True()
        {
            //Arrange
            long id = 1;

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsExistAsync(id);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void IsExist_By_Invalid_Id_Shold_Return_False()
        {
            //Arrange
            long id = -1;

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsExistAsync(id);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region IsUserDuplicate

        [Fact]
        public async void IsUserNameDuplicate_By_NonDuplicate_UserName_Shold_Return_False()
        {
            //Arrange
            string userName = "Unique";

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsUserNameDuplicateAsync(userName);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsUserNameDuplicate_By_Duplicate_UserName_Shold_Return_True()
        {
            //Arrange
            string userName = "U1";

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsUserNameDuplicateAsync(userName);

            //Assert
            result.Should().BeTrue();
        }

        #endregion

        #region IsUserNameDuplicate

        [Fact]
        public async void IsUserNameDuplicate_By_Id_And_NonDuplicate_UserName_Shold_Return_False()
        {
            //Arrange
            long id = 1;
            string userName = "Unique";

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsUserNameDuplicateAsync(id, userName);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsUserNameDuplicate_By_UserId_And_Duplicate_UserName_Shold_Return_True()
        {
            //Arrange
            long id = 2;
            string userName = "U1";

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsUserNameDuplicateAsync(id, userName);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsUserNameDuplicate_Exact_Record_Shold_Return_False()
        {
            //Arrange
            long id = 1;
            string userName = "U1";

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsUserNameDuplicateAsync(id, userName);

            //Assert
            result.Should().BeFalse();
        }
        #endregion

        #region IsPasswordValid

        [Fact]
        public async void IsPasswordValid_By_Valid_Password_Shold_Return_True()
        {
            //Arrange
            var changePasswordFormModel = UserMockDatas.changePasswordFormModel();

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsPasswordValidAsync(changePasswordFormModel.Id, changePasswordFormModel.oldPassword);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void IsPasswordValid_By_Invalid_Password_Shold_Return_False()
        {
            //Arrange
            var changePasswordFormModel = UserMockDatas.changePasswordFormModel("@@@");

            _mockUserSet.IqueryableRegisteration(UserMockDatas.UserCollectionEntityModels().AsQueryable());
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new UserService(_mockContext.Object);
            var result = await sut.IsPasswordValidAsync(changePasswordFormModel.Id, changePasswordFormModel.oldPassword);

            //Assert
            result.Should().BeFalse();
        }

        #endregion


        #region ChangePassword

        [Fact]
        public async void ChangePassword_Update_A_User_Password_Via_Context()
        {
            //Arrange
            var changePasswordFormModel = UserMockDatas.changePasswordFormModel("123", "123");
            _mockContext.Setup(u => u.Users).Returns(_mockUserSet.Object);
            _mockUserSet.Setup(m => m.FindAsync(changePasswordFormModel.Id)).ReturnsAsync(UserMockDatas.EntityModel());

            //Act
            var sut = new UserService(_mockContext.Object);
            await sut.ChangePasswordAsync(changePasswordFormModel);

            //Assert
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}
