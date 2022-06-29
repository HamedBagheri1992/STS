using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using STS.Interfaces.Contracts;
using STS.Tests.MockDatas;
using STS.WebApi.Controllers.V1;
using Xunit;

namespace STS.Tests.Systems.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountService;

        public AccountControllerTests()
        {
            _accountService = new Mock<IAccountService>();
        }

        #region Login

        [Fact]
        public async void Login_Should_Return_Status_200()
        {
            //Arrange
            var formModel = AccountMockDatas.ValidLoginFormModel();
            var userIdentityBase = AccountMockDatas.LoginUserIdentityBaseModel();

            _accountService.Setup(a => a.LoginAsync(formModel)).ReturnsAsync(userIdentityBase);
            _accountService.Setup(a => a.GenerateToken(userIdentityBase)).Returns(It.IsAny<string>());
            _accountService.Setup(a => a.UpdateLastLoginAsync(userIdentityBase));

            var sut = new AccountController(_accountService.Object);

            //Act
            var actionResult = await sut.Login(formModel);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Login_By_Invalid_UserName_And_Password_Should_Return_Status_404()
        {
            //Arrange
            var formModel = AccountMockDatas.InvalidLoginFormModel();
            _accountService.Setup(a => a.LoginAsync(formModel)).ReturnsAsync(() => null);

            var sut = new AccountController(_accountService.Object);

            //Act
            var actionResult = await sut.Login(formModel);
            var actionStatus = actionResult.Result as NotFoundObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void Login_By_Deactivated_User_Should_Return_Status_400()
        {
            //Arrange
            var formModel = AccountMockDatas.InvalidLoginFormModel();
            var userIdentityBase = AccountMockDatas.LoginDeactivatedUserIdentityBaseModel();
            _accountService.Setup(a => a.LoginAsync(formModel)).ReturnsAsync(userIdentityBase);
            var sut = new AccountController(_accountService.Object);

            //Act
            var actionResult = await sut.Login(formModel);
            var actionStatus = actionResult.Result as BadRequestObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(400);
        }

        [Fact]
        public async void Login_By_Expired_User_Should_Return_Status_400()
        {
            //Arrange
            var formModel = AccountMockDatas.ValidLoginFormModel();
            var userIdentityBase = AccountMockDatas.LoginExpiredUserIdentityBaseModel();

            _accountService.Setup(a => a.LoginAsync(formModel)).ReturnsAsync(userIdentityBase);
            var sut = new AccountController(_accountService.Object);

            //Act
            var actionResult = await sut.Login(formModel);
            var actionStatus = actionResult.Result as BadRequestObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(400);
        }

        #endregion
    }
}
