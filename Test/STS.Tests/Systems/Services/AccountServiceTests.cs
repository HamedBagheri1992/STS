using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using STS.Common.Configuration;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.AccountModels.FormModels;
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
    public class AccountServiceTests
    {
        private Mock<STSDbContext> _mockContext;
        private Mock<DbSet<User>> _mockUserSet;
        private Mock<DbSet<Application>> _mockApplicationSet;
        private IOptionsMonitor<BearerTokensConfigurationModel> _mockMonitor;

        public AccountServiceTests()
        {
            _mockContext = new Mock<STSDbContext>();
            _mockUserSet = new Mock<DbSet<User>>();
            _mockApplicationSet = new Mock<DbSet<Application>>();

            var model = new BearerTokensConfigurationModel
            {
                Key = "STSSystem14012024Token",
                Issuer = "http://localhost:1100/",
                Audience = "Any",
                AccessTokenExpirationDays = 365,
                RefreshTokenExpirationDays = 700,
                AllowMultipleLoginsFromTheSameUser = true,
                AllowSignoutAllUserActiveClients = false
            };

            _mockMonitor = Mock.Of<IOptionsMonitor<BearerTokensConfigurationModel>>(i => i.CurrentValue == model);
        }


        #region Login

        [Fact]
        public async void Login_By_Invalid_UserName_And_Password_Return_Null()
        {
            //Arrang
            var loginFormModel = new LoginFormModel() { UserName = "Unique", Password = "123", AppId = 1, SecretKey = Guid.NewGuid() };

            var data = UserMockDatas.UserCollectionEntityModels().AsQueryable();
            _mockUserSet.IqueryableRegisteration(data);

            _mockContext.Setup(c => c.Users).Returns(_mockUserSet.Object);

            //Act
            var sut = new AccountService(_mockContext.Object, _mockMonitor);
            var result = await sut.LoginAsync(loginFormModel);

            //Assert
            result.Should().BeNull();
        }


        [Fact]
        public async void Login_By_UserName_And_Password_Return_UserIdentity()
        {
            //Arrang
            var loginFormModel = new LoginFormModel() { UserName = "U1", Password = "123", AppId = 1, SecretKey = Guid.NewGuid() };

            var data = AccountMockDatas.UserCollectionEntityModelForLogin(loginFormModel.SecretKey).AsQueryable();
            _mockUserSet.IqueryableRegisteration(data);

            var appData = ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable();
            _mockApplicationSet.IqueryableRegisteration(appData);

            _mockContext.Setup(c => c.Users).Returns(_mockUserSet.Object);
            _mockContext.Setup(c => c.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new AccountService(_mockContext.Object, _mockMonitor);
            var result = await sut.LoginAsync(loginFormModel);

            //Assert
            result.Should().NotBeNull();
            result.UserName.Should().Be(loginFormModel.UserName);
        }

        #endregion

        #region GenerateToken

        [Fact]
        public void GenerateToken_Should_Return_Token()
        {
            //Arrang
            var userIdentity = AccountMockDatas.UserSingleIdentityBaseModel();

            //Act
            var sut = new AccountService(_mockContext.Object, _mockMonitor);
            var result = sut.GenerateToken(userIdentity);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Length.Should().BeGreaterThan(10);
        }

        #endregion

        #region UpdateLastLoginAsync

        [Fact]
        public async void UpdateLastLogin_Should_Call_Save_Changes()
        {
            //Arrang
            var userIdentity = AccountMockDatas.UserSingleIdentityBaseModel();
            var data = UserMockDatas.UserCollectionEntityModels().AsQueryable();
            _mockUserSet.IqueryableRegisteration(data);

            _mockContext.Setup(c => c.Users).Returns(_mockUserSet.Object);
            _mockUserSet.Setup(m => m.FindAsync(userIdentity.Id)).ReturnsAsync(UserMockDatas.EntityModel());

            //Act
            var sut = new AccountService(_mockContext.Object, _mockMonitor);
            await sut.UpdateLastLoginAsync(userIdentity);

            //Assert
            _mockContext.Verify(c => c.Users, Times.Once());
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}
