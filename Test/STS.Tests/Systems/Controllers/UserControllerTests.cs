using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using STS.Common.BaseModels;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Tests.MockDatas;
using STS.WebApi.Controllers.V1;
using System;
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

        #region Post

        [Fact]
        public async void Post_Should_Return_Status_201()
        {
            //Arrang
            var addFormModel = UserMockDatas.AddFormModel();
            var addedId = 1;

            _userService.Setup(u => u.IsUserNameDuplicateAsync(addFormModel.UserName)).ReturnsAsync(false);
            _userService.Setup(u => u.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _userService.Setup(u => u.GetAsync(addedId)).ReturnsAsync(UserMockDatas.UserSingleViewModel(addFormModel));

            var sut = new UserController(_userService.Object);


            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as CreatedAtActionResult;

            //Assert
            _userService.Verify(ur => ur.AddAsync(addFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async void Post_Should_Return_Added_User()
        {
            //Arrang
            var addFormModel = UserMockDatas.AddFormModel();
            var addedId = 1;

            _userService.Setup(u => u.IsUserNameDuplicateAsync(addFormModel.UserName)).ReturnsAsync(false);
            _userService.Setup(u => u.GetAsync(addedId)).ReturnsAsync(UserMockDatas.UserSingleViewModel(addFormModel));

            _userService.Setup(u => u.AddAsync(addFormModel)).ReturnsAsync(addedId);
            var sut = new UserController(_userService.Object);


            //Act
            var actionResult = (await sut.Post(addFormModel)) as CreatedAtActionResult;
            var result = actionResult.Value as UserViewModel;

            //Assert            
            _userService.Verify(u => u.AddAsync(addFormModel), Times.Once);

            actionResult.Value.Should().NotBeNull();

            result.Id.Should().NotBe(0);
            result.UserName.Should().Be(addFormModel.UserName);
            result.FirstName.Should().Be(addFormModel.FirstName);
            result.LastName.Should().Be(addFormModel.LastName);
            result.CreatedDate.Should().BeBefore(DateTime.Now);
        }

        [Fact]
        public async void Post_Duplicate_UserName_Return_Status_400()
        {
            //Arrange
            var addFormModel = UserMockDatas.AddFormModel();
            var addedId = 1;

            _userService.Setup(u => u.IsUserNameDuplicateAsync(addFormModel.UserName)).ReturnsAsync(true);
            _userService.Setup(u => u.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _userService.Setup(u => u.GetAsync(addedId)).ReturnsAsync(UserMockDatas.UserSingleViewModel(addFormModel));

            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _userService.Verify(ur => ur.AddAsync(addFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Post_Invalid_UserName_Format_Return_Status_400()
        {
            //Arrange
            var addFormModel = UserMockDatas.AddFormModel();
            addFormModel.UserName = "U_1";
            var addedId = 1;

            _userService.Setup(u => u.IsUserNameDuplicateAsync(addFormModel.UserName)).ReturnsAsync(false);
            _userService.Setup(u => u.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _userService.Setup(u => u.GetAsync(addedId)).ReturnsAsync(UserMockDatas.UserSingleViewModel(addFormModel));

            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _userService.Verify(ur => ur.AddAsync(addFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        #endregion

        #region Put_Update

        [Fact]
        public async void Put_Should_Return_Status_204()
        {
            //Arrang
            var updateFormModel = UserMockDatas.UpdateFormModel();

            _userService.Setup(u => u.IsExistAsync(updateFormModel.Id)).ReturnsAsync(true);
            _userService.Setup(u => u.UpdateAsync(updateFormModel));

            var sut = new UserController(_userService.Object);


            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as NoContentResult;

            //Assert
            _userService.Verify(ur => ur.UpdateAsync(updateFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void Put_Invalid_User_Should_Return_Status_400()
        {
            //Arrange
            var updateFormModel = UserMockDatas.UpdateFormModel();

            _userService.Setup(u => u.IsExistAsync(updateFormModel.Id)).ReturnsAsync(false);
            _userService.Setup(u => u.UpdateAsync(updateFormModel));

            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _userService.Verify(ur => ur.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_Duplicate_UserName_Should_Return_Status_400()
        {
            //Arrange
            var updateFormModel = UserMockDatas.UpdateFormModel();

            _userService.Setup(u => u.IsExistAsync(updateFormModel.Id)).ReturnsAsync(true);
            _userService.Setup(u => u.IsUserNameDuplicateAsync(updateFormModel.Id, updateFormModel.UserName)).ReturnsAsync(true);

            _userService.Setup(u => u.UpdateAsync(updateFormModel));
            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _userService.Verify(ur => ur.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_Invalid_UserName_Format_Should_Return_Status_400()
        {
            //Arrange
            var updateFormModel = UserMockDatas.UpdateFormModel();
            updateFormModel.UserName = "U_1";

            _userService.Setup(u => u.IsExistAsync(updateFormModel.Id)).ReturnsAsync(true);
            _userService.Setup(u => u.IsUserNameDuplicateAsync(updateFormModel.Id, updateFormModel.UserName)).ReturnsAsync(false);

            _userService.Setup(u => u.UpdateAsync(updateFormModel));
            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _userService.Verify(ur => ur.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        #endregion

        #region Put_ChangePassword

        [Fact]
        public async void ChangePassword_Should_Return_Status_204()
        {
            //Arrang
            var changePasswordFormModel = UserMockDatas.changePasswordFormModel();

            _userService.Setup(u => u.IsPasswordValidAsync(changePasswordFormModel.Id, changePasswordFormModel.oldPassword)).ReturnsAsync(true);
            _userService.Setup(u => u.ChangePasswordAsync(changePasswordFormModel));

            var sut = new UserController(_userService.Object);


            //Act
            var actionResult = await sut.ChangePassword(changePasswordFormModel);
            var result = actionResult as NoContentResult;

            //Assert
            _userService.Verify(ur => ur.ChangePasswordAsync(changePasswordFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void ChangePassword_Invalid_Password_Should_Return_Status_400()
        {
            //Arrange
            var changePasswordFormModel = UserMockDatas.changePasswordFormModel();

            _userService.Setup(u => u.IsPasswordValidAsync(changePasswordFormModel.Id, changePasswordFormModel.oldPassword)).ReturnsAsync(false);
            _userService.Setup(u => u.ChangePasswordAsync(changePasswordFormModel));

            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.ChangePassword(changePasswordFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _userService.Verify(ur => ur.ChangePasswordAsync(changePasswordFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        #endregion

        #region Delete

        [Fact]
        public async void Delete_Should_Return_Status_204()
        {
            //Arrang
            long id = 1;
            _userService.Setup(u => u.IsExistAsync(id)).ReturnsAsync(true);
            _userService.Setup(u => u.DeleteAsync(id));

            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Delete(id);
            var result = actionResult as NoContentResult;

            //Assert
            _userService.Verify(ap => ap.DeleteAsync(id), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void Delete_Invalid_User_Return_Status_400()
        {
            //Arrange
            long id = 1;
            _userService.Setup(u => u.IsExistAsync(id)).ReturnsAsync(false);

            _userService.Setup(u => u.DeleteAsync(id));
            var sut = new UserController(_userService.Object);

            //Act
            var actionResult = await sut.Delete(id);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _userService.Verify(ap => ap.DeleteAsync(id), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        #endregion
    }
}
