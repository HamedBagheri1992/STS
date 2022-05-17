using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using STS.Common.BaseModels;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Tests.MockDatas;
using STS.WebApi.Controllers.V1;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace STS.Tests.Systems.Controllers
{
    public class PermissionControllerTests
    {
        private readonly Mock<IPermissionService> _permissionService;
        private readonly Mock<IRoleService> _roleService;

        public PermissionControllerTests()
        {
            _permissionService = new Mock<IPermissionService>();
            _roleService = new Mock<IRoleService>();
        }

        #region Get_By_RoleId
        [Fact]
        public async void Get_By_RoleId_Should_Return_Status_200()
        {
            //Arrange
            int roleId = 1;
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Get(roleId);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_By_RoleId_Should_Return_Permission_Collection()
        {
            //Arrange
            int roleId = 1;
            _permissionService.Setup(p => p.GetAsync(roleId)).ReturnsAsync(PermissionMockDatas.PermissionCollectionViewModels());
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Get(roleId);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var permissions = result.Value as IEnumerable<PermissionViewModel>;
            permissions.Should().HaveCount(PermissionMockDatas.PermissionCollectionViewModels().Count());

        }

        #endregion


        #region Get_By_RoleId_And_PermissionId

        [Fact]
        public async void Get_By_RoleId_And_PermissionId_Should_Return_Status_200()
        {
            //Arrange
            int roleId = 1;
            int permissionId = 1;
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Get(roleId, permissionId);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_By_RoleId_And_PermissionId_Should_Return_Permission_One_Permission()
        {
            //Arrange
            int roleId = 1;
            int permissionId = 1;
            var mockPermission = PermissionMockDatas.PermissionSingleViewModel();

            _permissionService.Setup(p => p.GetAsync(roleId, permissionId)).ReturnsAsync(mockPermission);
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Get(roleId, permissionId);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var permission = result.Value as PermissionViewModel;
            permission.Id.Should().Be(mockPermission.Id);
            permission.Title.Should().Be(mockPermission.Title);
            permission.DisplayTitle.Should().Be(mockPermission.DisplayTitle);

        }

        #endregion


        #region Post

        [Fact]
        public async void Post_Should_Return_Status_201()
        {
            //Arrang
            var addFormModel = PermissionMockDatas.AddFormModel();
            var addedId = 1;

            _roleService.Setup(r => r.IsExistAsync(addFormModel.RoleId)).ReturnsAsync(true);

            _permissionService.Setup(r => r.IsTitleDuplicateAsync(addFormModel.Title)).ReturnsAsync(false);
            _permissionService.Setup(p => p.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _permissionService.Setup(p => p.GetAsync(addFormModel.RoleId, addedId)).ReturnsAsync(PermissionMockDatas.PermissionSingleViewModel(addFormModel));

            var sut = new PermissionController(_permissionService.Object, _roleService.Object);


            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as CreatedAtActionResult;

            //Assert
            _permissionService.Verify(per => per.AddAsync(addFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);

        }

        [Fact]
        public async void Post_Should_Return_Added_Permission()
        {
            //Arrang
            var addFormModel = PermissionMockDatas.AddFormModel();
            var addedId = 1;

            _roleService.Setup(r => r.IsExistAsync(addFormModel.RoleId)).ReturnsAsync(true);
            _permissionService.Setup(r => r.IsTitleDuplicateAsync(addFormModel.Title)).ReturnsAsync(false);
            _permissionService.Setup(p => p.GetAsync(addFormModel.RoleId, addedId)).ReturnsAsync(PermissionMockDatas.PermissionSingleViewModel(addFormModel));

            _permissionService.Setup(p => p.AddAsync(addFormModel)).ReturnsAsync(addedId);
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);


            //Act
            var actionResult = (await sut.Post(addFormModel)) as CreatedAtActionResult;
            var result = actionResult.Value as PermissionViewModel;

            //Assert            
            _permissionService.Verify(p => p.AddAsync(addFormModel), Times.Once);

            actionResult.Value.Should().NotBeNull();

            result.Id.Should().NotBe(0);
            result.Title.Should().Be(addFormModel.Title);
            result.DisplayTitle.Should().Be(addFormModel.DisplayTitle);
            result.RoleId.Should().Be(addFormModel.RoleId);
        }

        [Fact]
        public async void Post_Invalid_RoleId_Return_Status_400()
        {
            //Arrange
            var addFormModel = PermissionMockDatas.AddFormModel();
            var addedId = 1;

            _roleService.Setup(r => r.IsExistAsync(addFormModel.RoleId)).ReturnsAsync(false);
            _permissionService.Setup(r => r.IsTitleDuplicateAsync(addFormModel.Title)).ReturnsAsync(false);
            _permissionService.Setup(p => p.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _permissionService.Setup(p => p.GetAsync(addFormModel.RoleId, addedId)).ReturnsAsync(PermissionMockDatas.PermissionSingleViewModel(addFormModel));


            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _permissionService.Verify(per => per.AddAsync(addFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Post_Duplicate_Title_Return_Status_400()
        {
            //Arrange
            var addFormModel = PermissionMockDatas.AddFormModel();
            var addedId = 1;

            _roleService.Setup(r => r.IsExistAsync(addFormModel.RoleId)).ReturnsAsync(true);
            _permissionService.Setup(r => r.IsTitleDuplicateAsync(addFormModel.Title)).ReturnsAsync(true);
            _permissionService.Setup(p => p.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _permissionService.Setup(p => p.GetAsync(addFormModel.RoleId, addedId)).ReturnsAsync(PermissionMockDatas.PermissionSingleViewModel(addFormModel));

            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _permissionService.Verify(per => per.AddAsync(addFormModel), Times.Never);
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
            var updateFormModel = PermissionMockDatas.UpdateFormModel();

            _roleService.Setup(r => r.IsExistAsync(updateFormModel.RoleId)).ReturnsAsync(true);
            _permissionService.Setup(p => p.IsPermissionValidAsync(updateFormModel.Id)).ReturnsAsync(true);

            _permissionService.Setup(p => p.UpdateAsync(updateFormModel));
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);


            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as NoContentResult;

            //Assert
            _permissionService.Verify(per => per.UpdateAsync(updateFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);

        }

        [Fact]
        public async void Put_Invalid_RoleId_Return_Status_400()
        {
            //Arrange
            var updateFormModel = PermissionMockDatas.UpdateFormModel();

            _roleService.Setup(r => r.IsExistAsync(updateFormModel.RoleId)).ReturnsAsync(false);
            _permissionService.Setup(p => p.IsPermissionValidAsync(updateFormModel.Id)).ReturnsAsync(true);

            _permissionService.Setup(p => p.UpdateAsync(updateFormModel));
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _permissionService.Verify(per => per.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_Invalid_Permission_Return_Status_400()
        {
            //Arrange
            var updateFormModel = PermissionMockDatas.UpdateFormModel();

            _roleService.Setup(r => r.IsExistAsync(updateFormModel.RoleId)).ReturnsAsync(true);
            _permissionService.Setup(p => p.IsPermissionValidAsync(updateFormModel.Id)).ReturnsAsync(false);

            _permissionService.Setup(p => p.UpdateAsync(updateFormModel));
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _permissionService.Verify(per => per.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_Duplicate_Title_Return_Status_400()
        {
            //Arrange
            var updateFormModel = PermissionMockDatas.UpdateFormModel();

            _roleService.Setup(r => r.IsExistAsync(updateFormModel.RoleId)).ReturnsAsync(true);
            _permissionService.Setup(p => p.IsPermissionValidAsync(updateFormModel.Id)).ReturnsAsync(true);
            _permissionService.Setup(p => p.IsTitleDuplicateAsync(updateFormModel.Id, updateFormModel.Title)).ReturnsAsync(true);

            _permissionService.Setup(p => p.UpdateAsync(updateFormModel));
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _permissionService.Verify(per => per.UpdateAsync(updateFormModel), Times.Never);
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
            _permissionService.Setup(p => p.IsPermissionValidAsync(id)).ReturnsAsync(true);

            _permissionService.Setup(p => p.DeleteAsync(id));
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);


            //Act
            var actionResult = await sut.Delete(id);
            var result = actionResult as NoContentResult;

            //Assert
            _permissionService.Verify(per => per.DeleteAsync(id), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void Delete_Invalid_Permission_Return_Status_400()
        {
            //Arrange
            long id = 1;
            _permissionService.Setup(p => p.IsPermissionValidAsync(id)).ReturnsAsync(false);

            _permissionService.Setup(p => p.DeleteAsync(id));
            var sut = new PermissionController(_permissionService.Object, _roleService.Object);

            //Act
            var actionResult = await sut.Delete(id);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _permissionService.Verify(per => per.DeleteAsync(id), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        #endregion
    }
}
