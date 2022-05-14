using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

        public PermissionControllerTests()
        {
            _permissionService = new Mock<IPermissionService>();
        }

        #region Get_By_RoleId
        [Fact]
        public async void Get_By_RoleId_Should_Return_Status_200()
        {
            //Arrange
            int roleId = 1;
            var sut = new PermissionController(_permissionService.Object);

            //Act
            var actionResult = await sut.Get(roleId);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            Assert.NotNull(actionResult);
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_By_RoleId_Should_Return_Permission_Collection()
        {
            //Arrange
            int roleId = 1;
            _permissionService.Setup(p => p.GetAsync(roleId)).ReturnsAsync(PermissionMockDatas.PermissionCollectionViewModels());
            var sut = new PermissionController(_permissionService.Object);

            //Act
            var actionResult = await sut.Get(roleId);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            Assert.NotNull(result);
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
            var sut = new PermissionController(_permissionService.Object);

            //Act
            var actionResult = await sut.Get(roleId, permissionId);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            Assert.NotNull(actionResult);
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
            var sut = new PermissionController(_permissionService.Object);

            //Act
            var actionResult = await sut.Get(roleId, permissionId);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            Assert.NotNull(result);
            var permission = result.Value as PermissionViewModel;
            permission.Id.Should().Be(mockPermission.Id);
            permission.Title.Should().Be(mockPermission.Title);
            permission.DisplayTitle.Should().Be(mockPermission.DisplayTitle);

        }

        #endregion


        #region Post

        [Fact]
        public async void Post_Should_Return_201()
        {
            //Arrang
            var addFormModel = new AddPermissionFormModel { Title = "Permission_1", DisplayTitle = "مجوز_1" };
            _permissionService.Setup(p => p.AddAsync(addFormModel)).ReturnsAsync(PermissionMockDatas.PermissionSingleViewModel(addFormModel));
            var sut = new PermissionController(_permissionService.Object);


            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as CreatedAtActionResult;

            //Assert
            Assert.NotNull(result);
            result.StatusCode.Should().Be(201);

        }

        [Fact]
        public async void Post_Should_Return_Added_Permission()
        {
            //Arrang
            var addFormModel = new AddPermissionFormModel { Title = "Permission_1", DisplayTitle = "مجوز_1" };
            _permissionService.Setup(p => p.AddAsync(addFormModel)).ReturnsAsync(PermissionMockDatas.PermissionSingleViewModel(addFormModel));
            var sut = new PermissionController(_permissionService.Object);


            //Act
            var actionResult = (await sut.Post(addFormModel)) as CreatedAtActionResult;
            var result = actionResult.Value as PermissionViewModel;

            //Assert            
            actionResult.Value.Should().NotBeNull();

            result.Id.Should().NotBe(0);
            result.Title.Should().Be(addFormModel.Title);
            result.DisplayTitle.Should().Be(addFormModel.DisplayTitle);
            result.RoleId.Should().Be(addFormModel.RoleId);
        }

        #endregion
    }
}
