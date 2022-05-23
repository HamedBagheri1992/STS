using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using STS.Common.BaseModels;
using STS.DTOs.RoleModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Tests.MockDatas;
using STS.WebApi.Controllers.V1;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace STS.Tests.Systems.Controllers
{
    public class RoleControllerTests
    {
        private readonly Mock<IApplicationService> _applicationService;
        private readonly Mock<IRoleService> _roleService;

        public RoleControllerTests()
        {
            _applicationService = new Mock<IApplicationService>();
            _roleService = new Mock<IRoleService>();
        }

        #region Get_By_ApplicationId
        [Fact]
        public async void Get_By_AppllicationId_Should_Return_Status_200()
        {
            //Arrange
            int applicationId = 1;
            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Get(applicationId);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_By_ApplicationId_Should_Return_Role_Collection()
        {
            //Arrange
            int applicationId = 1;
            _roleService.Setup(r => r.GetAsync(applicationId)).ReturnsAsync(RoleMockDatas.RoleCollectionViewModels());
            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Get(applicationId);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var roles = result.Value as List<RoleViewModel>;
            roles.Should().HaveCount(RoleMockDatas.RoleCollectionViewModels().Count());

        }

        #endregion


        #region Get_By_ApplicationId_And_RoleId

        [Fact]
        public async void Get_By_ApplicationId_And_RoleId_Should_Return_Status_200()
        {
            //Arrange
            int roleId = 1;
            int applicationId = 1;
            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Get(applicationId, roleId);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_By_ApplicationId_And_RoleId_Should_Return_One_Role()
        {
            //Arrange
            int roleId = 1;
            int applicationId = 1;
            var mockRole = RoleMockDatas.RoleSingleViewModel();

            _roleService.Setup(r => r.GetAsync(applicationId, roleId)).ReturnsAsync(mockRole);
            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Get(applicationId, roleId);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var role = result.Value as RoleViewModel;
            role.Id.Should().Be(mockRole.Id);
            role.Caption.Should().Be(mockRole.Caption);
            role.ApplicationId.Should().Be(mockRole.ApplicationId);

        }

        #endregion


        #region Post

        [Fact]
        public async void Post_Should_Return_Status_201()
        {
            //Arrang
            var addFormModel = RoleMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(r => r.IsExistAsync(addFormModel.ApplicationId)).ReturnsAsync(true);

            _roleService.Setup(r => r.IsCaptionDuplicateAsync(addFormModel.Caption)).ReturnsAsync(false);
            _roleService.Setup(r => r.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _roleService.Setup(r => r.GetAsync(addFormModel.ApplicationId, addedId)).ReturnsAsync(RoleMockDatas.RoleSingleViewModel(addFormModel));

            var sut = new RoleController(_applicationService.Object, _roleService.Object);


            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as CreatedAtActionResult;

            //Assert
            _roleService.Verify(rl => rl.AddAsync(addFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);

        }

        [Fact]
        public async void Post_Should_Return_Added_Role()
        {
            //Arrang
            var addFormModel = RoleMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(r => r.IsExistAsync(addFormModel.ApplicationId)).ReturnsAsync(true);
            _roleService.Setup(r => r.IsCaptionDuplicateAsync(addFormModel.Caption)).ReturnsAsync(false);
            _roleService.Setup(r => r.GetAsync(addFormModel.ApplicationId, addedId)).ReturnsAsync(RoleMockDatas.RoleSingleViewModel(addFormModel));

            _roleService.Setup(r => r.AddAsync(addFormModel)).ReturnsAsync(addedId);
            var sut = new RoleController(_applicationService.Object, _roleService.Object);


            //Act
            var actionResult = (await sut.Post(addFormModel)) as CreatedAtActionResult;
            var result = actionResult.Value as RoleViewModel;

            //Assert            
            _roleService.Verify(r => r.AddAsync(addFormModel), Times.Once);

            actionResult.Value.Should().NotBeNull();

            result.Id.Should().NotBe(0);
            result.Caption.Should().Be(addFormModel.Caption);
            result.ApplicationId.Should().Be(addFormModel.ApplicationId);
        }

        [Fact]
        public async void Post_Invalid_ApplicationId_Return_Status_400()
        {
            //Arrange
            var addFormModel = RoleMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(r => r.IsExistAsync(addFormModel.ApplicationId)).ReturnsAsync(false);
            _roleService.Setup(r => r.IsCaptionDuplicateAsync(addFormModel.Caption)).ReturnsAsync(false);
            _roleService.Setup(r => r.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _roleService.Setup(r => r.GetAsync(addFormModel.ApplicationId, addedId)).ReturnsAsync(RoleMockDatas.RoleSingleViewModel(addFormModel));


            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _roleService.Verify(rl => rl.AddAsync(addFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Post_Duplicate_Caption_Return_Status_400()
        {
            //Arrange
            var addFormModel = RoleMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(r => r.IsExistAsync(addFormModel.ApplicationId)).ReturnsAsync(true);
            _roleService.Setup(r => r.IsCaptionDuplicateAsync(addFormModel.Caption)).ReturnsAsync(true);
            _roleService.Setup(r => r.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _roleService.Setup(r => r.GetAsync(addFormModel.ApplicationId, addedId)).ReturnsAsync(RoleMockDatas.RoleSingleViewModel(addFormModel));

            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _roleService.Verify(rl => rl.AddAsync(addFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        #endregion


        #region Put

        [Fact]
        public async void Put_Should_Return_Status_204()
        {
            //Arrang
            var updateFormModel = RoleMockDatas.UpdateFormModel();

            _applicationService.Setup(r => r.IsExistAsync(updateFormModel.ApplicationId)).ReturnsAsync(true);
            _roleService.Setup(r => r.IsRoleValidAsync(updateFormModel.Id)).ReturnsAsync(true);

            _roleService.Setup(r => r.UpdateAsync(updateFormModel));
            var sut = new RoleController(_applicationService.Object, _roleService.Object);


            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as NoContentResult;

            //Assert
            _roleService.Verify(rl => rl.UpdateAsync(updateFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);

        }

        [Fact]
        public async void Put_Invalid_ApplicationId_Return_Status_400()
        {
            //Arrange
            var updateFormModel = RoleMockDatas.UpdateFormModel();

            _applicationService.Setup(r => r.IsExistAsync(updateFormModel.ApplicationId)).ReturnsAsync(false);
            _roleService.Setup(r => r.IsRoleValidAsync(updateFormModel.Id)).ReturnsAsync(true);

            _roleService.Setup(r => r.UpdateAsync(updateFormModel));
            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _roleService.Verify(rl => rl.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_Invalid_Role_Return_Status_400()
        {
            //Arrange
            var updateFormModel = RoleMockDatas.UpdateFormModel();

            _applicationService.Setup(r => r.IsExistAsync(updateFormModel.ApplicationId)).ReturnsAsync(true);
            _roleService.Setup(r => r.IsRoleValidAsync(updateFormModel.Id)).ReturnsAsync(false);

            _roleService.Setup(r => r.UpdateAsync(updateFormModel));
            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _roleService.Verify(rl => rl.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_Duplicate_Caption_Return_Status_400()
        {
            //Arrange
            var updateFormModel = RoleMockDatas.UpdateFormModel();

            _applicationService.Setup(r => r.IsExistAsync(updateFormModel.ApplicationId)).ReturnsAsync(true);
            _roleService.Setup(r => r.IsRoleValidAsync(updateFormModel.Id)).ReturnsAsync(true);
            _roleService.Setup(r => r.IsCaptionDuplicateAsync(updateFormModel.Id, updateFormModel.Caption)).ReturnsAsync(true);

            _roleService.Setup(r => r.UpdateAsync(updateFormModel));
            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _roleService.Verify(rl => rl.UpdateAsync(updateFormModel), Times.Never);
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
            _roleService.Setup(r => r.IsRoleValidAsync(id)).ReturnsAsync(true);

            _roleService.Setup(r => r.DeleteAsync(id));
            var sut = new RoleController(_applicationService.Object, _roleService.Object);


            //Act
            var actionResult = await sut.Delete(id);
            var result = actionResult as NoContentResult;

            //Assert
            _roleService.Verify(rl => rl.DeleteAsync(id), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void Delete_Invalid_Role_Return_Status_400()
        {
            //Arrange
            long id = 1;
            _roleService.Setup(r => r.IsRoleValidAsync(id)).ReturnsAsync(false);

            _roleService.Setup(r => r.DeleteAsync(id));
            var sut = new RoleController(_applicationService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Delete(id);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _roleService.Verify(rl => rl.DeleteAsync(id), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        #endregion
    }
}
