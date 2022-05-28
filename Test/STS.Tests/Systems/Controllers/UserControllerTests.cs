using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Tests.MockDatas;
using STS.WebApi.Controllers.V1;
using Xunit;

namespace STS.Tests.Systems.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userService;

        public UserControllerTests()
        {
            _userService = new Mock<IUserService>();
        }

        #region Get

        [Fact]
        public async void Get_Should_Return_Status_200()
        {
            //Arrange
            var pagination = new PaginationParam { PageNumber = 1, PageSize = 10 };
            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Get(pagination);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_Should_Return_Paged_Collection()
        {
            //Arrange
            var pagination = new PaginationParam { PageNumber = 1, PageSize = 10 };
            var mockUser = UserMockDatas.UserPagedCollectionViewModels(pagination);

            _userService.Setup(u => u.GetAsync(pagination)).ReturnsAsync(mockUser);
            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Get(pagination);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var user = result.Value as PaginatedResult<UserViewModel>;
            user.Items.Should().NotBeNull();
            user.Items.Should().HaveCount(mockUser.Items.Count);
        }


        [Fact]
        public async void Get_By_Id_Should_Return_Status_200()
        {
            //Arrange
            int id = 1;
            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Get(id);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_By_Id_Should_Return_Single_User()
        {
            //Arrange
            long id = 1;
            _userService.Setup(u => u.GetAsync(id)).ReturnsAsync(UserMockDatas.UserSingleViewModel());
            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Get(id);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var user = result.Value as UserViewModel;
            user.Id.Should().NotBe(0);
            user.FirstName.Should().NotBeNull();
        }


        [Fact]
        public async void Get_By_Invalid_Id_Should_Return_Null()
        {
            //Arrange
            long id = -1;
            _userService.Setup(u => u.GetAsync(id)).ReturnsAsync(() => null);
            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Get(id);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            result.Value.Should().BeNull();
        }
        #endregion
    }
}
