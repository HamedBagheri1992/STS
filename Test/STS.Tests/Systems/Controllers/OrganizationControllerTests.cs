using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using STS.Common.BaseModels;
using STS.DTOs.OrganizationModel.ViewModels;
using STS.Interfaces.Contracts;
using STS.Tests.MockDatas;
using STS.WebApi.Controllers.V1;
using System.Collections.Generic;
using Xunit;

namespace STS.Tests.Systems.Controllers
{
    public class OrganizationControllerTests
    {
        private readonly Mock<IOrganizationService> _organizationService;
        private readonly Mock<IUserService> _userService;

        public OrganizationControllerTests()
        {
            _organizationService = new Mock<IOrganizationService>();
            _userService = new Mock<IUserService>();
        }


        #region Get

        [Fact]
        public async void Get_Should_Return_Status_200()
        {
            //Arrange
            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.Get();
            var result = actionResult.Result as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_Should_Return_Organization_Collection()
        {
            //Arrange
            var datas = OrganizationMockDatas.OrganizationCollectionViewModels();
            _organizationService.Setup(o => o.GetAsync()).ReturnsAsync(datas);
            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.Get();
            var result = actionResult.Result as OkObjectResult;

            //Assert            
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var orgs = result.Value as List<OrganizationViewModel>;
            orgs.Should().HaveCount(datas.Count);
        }


        [Fact]
        public async void Get_OrgId_Should_Return_Status_200()
        {
            //Arrange
            long orgId = 1;
            var datas = OrganizationMockDatas.OrganizationCollectionViewModels();
            _organizationService.Setup(o => o.GetAsync(orgId)).ReturnsAsync(datas);
            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.Get(orgId);
            var result = actionResult.Result as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_OrgId_Should_Return_Organization_Collection()
        {
            //Arrange
            long orgId = 1;
            var datas = OrganizationMockDatas.OrganizationCollectionViewModels();
            _organizationService.Setup(o => o.GetAsync(orgId)).ReturnsAsync(datas);
            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.Get(orgId);
            var result = actionResult.Result as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var orgs = result.Value as List<OrganizationViewModel>;
            orgs.Should().HaveCount(datas.Count);
        }

        #endregion

        #region GetSingleOrganization

        [Fact]
        public async void GetSingleOrganization_By_OrgId_Should_Return_Single_Organization()
        {
            //Arrange
            long orgId = 1;
            var organization = OrganizationMockDatas.OrganizationSingleViewModels(orgId);
            _organizationService.Setup(o => o.GetSingleOrganizationAsync(orgId)).ReturnsAsync(organization);
            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.GetSingleOrganization(orgId);
            var result = actionResult.Result as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var org = result.Value as OrganizationViewModel;
            org.Should().NotBeNull();
            org.Id.Should().Be(orgId);
            org.Title.Should().Be(organization.Title);
        }

        [Fact]
        public async void GetSingleOrganization_By_Invalid_OrgId_Should_Return_Status_404()
        {
            //Arrange
            long orgId = 1;
            _organizationService.Setup(o => o.GetSingleOrganizationAsync(orgId)).ReturnsAsync(() => null);
            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.GetSingleOrganization(orgId);
            var result = actionResult.Result as NotFoundObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);

            result.Value.Should().BeOfType<ErrorDetails>();
        }

        #endregion

        #region GetUserOrganizations

        [Fact]
        public async void GetUserOrganizations_By_UserId_Should_Return_Organization_Collection()
        {
            //Arrange
            long userId = 1;

            var user = UserMockDatas.UserSingleViewModel();
            _userService.Setup(u => u.GetAsync(userId)).ReturnsAsync(user);

            var datas = OrganizationMockDatas.OrganizationCollectionViewModels();
            _organizationService.Setup(o => o.GetAsync(user)).ReturnsAsync(datas);

            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.GetUserOrganizations(userId);
            var result = actionResult.Result as OkObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var orgs = result.Value as List<OrganizationViewModel>;
            orgs.Should().HaveCount(datas.Count);
        }

        [Fact]
        public async void GetUserOrganizations_By_Invalid_UserId_Should_Return_Status_400()
        {
            //Arrange
            long userId = 1;
            _userService.Setup(u => u.GetAsync(userId)).ReturnsAsync(() => null);

            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.GetUserOrganizations(userId);
            var result = actionResult.Result as BadRequestObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType<ErrorDetails>();
        }

        [Fact]
        public async void GetUserOrganizations_By_Zero_OrganizatioIds_Should_Return_Status_404()
        {
            //Arrange
            long userId = 1;
            var user = UserMockDatas.UserSingleViewModel();
            user.OrganizationIds = new List<long> { };
            _userService.Setup(u => u.GetAsync(userId)).ReturnsAsync(user);

            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.GetUserOrganizations(userId);
            var result = actionResult.Result as NotFoundObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            result.Value.Should().BeOfType<ErrorDetails>();
        }

        #endregion

        #region Post

        [Fact]
        public async void Post_Without_ParentId_Should_Return_Status_201()
        {
            //Arrang         
            long addedId = 1;
            var addedOrg = OrganizationMockDatas.OrganizationSingleViewModels(addedId);
            var addFormModel = OrganizationMockDatas.GetAddFormModel(null);

            _organizationService.Setup(o => o.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _organizationService.Setup(o => o.GetSingleOrganizationAsync(addedId)).ReturnsAsync(addedOrg);
            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = (await sut.Post(addFormModel)) as CreatedAtActionResult;
            var result = actionResult.Value as OrganizationViewModel;

            //Assert
            result.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(201);
            result.Title.Should().Be(addedOrg.Title);
        }


        [Fact]
        public async void Post_By_ParentId_Should_Return_Status_201()
        {
            //Arrang
            long parentId = 1;
            long addedId = 2;
            var addedOrg = OrganizationMockDatas.OrganizationSingleViewModels(addedId);
            var addFormModel = OrganizationMockDatas.GetAddFormModel(parentId);

            _organizationService.Setup(o => o.IsExistAsync(parentId)).ReturnsAsync(true);
            _organizationService.Setup(o => o.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _organizationService.Setup(o => o.GetSingleOrganizationAsync(addedId)).ReturnsAsync(addedOrg);
            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = (await sut.Post(addFormModel)) as CreatedAtActionResult;
            var result = actionResult.Value as OrganizationViewModel;

            //Assert
            result.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(201);
            result.Title.Should().Be(addedOrg.Title);
        }


        [Fact]
        public async void Post_By_Invalid_ParentId_Should_Return_Status_400()
        {
            //Arrang
            long parentId = 1;
            long addedId = 2;
            var addedOrg = OrganizationMockDatas.OrganizationSingleViewModels(addedId);
            var addFormModel = OrganizationMockDatas.GetAddFormModel(parentId);

            _organizationService.Setup(o => o.IsExistAsync(parentId)).ReturnsAsync(false);

            var sut = new OrganizationController(_organizationService.Object, _userService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType<ErrorDetails>();
        }

        #endregion


    }
}
