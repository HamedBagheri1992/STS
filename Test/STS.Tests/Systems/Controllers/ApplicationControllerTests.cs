using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using STS.Common.BaseModels;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Interfaces.Contracts;
using STS.Tests.MockDatas;
using STS.WebApi.Controllers.V1;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace STS.Tests.Systems.Controllers
{
    public class ApplicationControllerTests
    {
        private readonly Mock<IApplicationService> _applicationService;

        public ApplicationControllerTests()
        {
            _applicationService = new Mock<IApplicationService>();
        }

        #region Get

        [Fact]
        public async void Get_Should_Return_Status_200()
        {
            //Arrange
            var pagination = new PaginationParam { PageNumber = 1, PageSize = 10 };
            var sut = new ApplicationController(_applicationService.Object);

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
            var mockApplication = ApplicationMockDatas.ApplicationPagedCollectionViewModels(pagination);

            _applicationService.Setup(a => a.GetAsync(pagination)).ReturnsAsync(mockApplication);
            var sut = new ApplicationController(_applicationService.Object);

            //Act
            var actionResult = await sut.Get(pagination);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var app = result.Value as PaginatedResult<ApplicationViewModel>;
            app.Items.Should().NotBeNull();
            app.Items.Should().HaveCount(mockApplication.Items.Count);
        }


        [Fact]
        public async void Get_By_Id_Should_Return_Status_200()
        {
            //Arrange
            int id = 1;
            var sut = new ApplicationController(_applicationService.Object);

            //Act
            var actionResult = await sut.Get(id);
            var actionStatus = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void Get_By_Id_Should_Return_Single_Application()
        {
            //Arrange
            long id = 1;
            _applicationService.Setup(a => a.GetAsync(id)).ReturnsAsync(ApplicationMockDatas.ApplicationSingleViewModel());
            var sut = new ApplicationController(_applicationService.Object);

            //Act
            var actionResult = await sut.Get(id);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var app = result.Value as ApplicationViewModel;
            app.Id.Should().NotBe(0);
            app.Title.Should().NotBeNull();
        }


        [Fact]
        public async void Get_By_Invalid_Id_Should_Return_Null()
        {
            //Arrange
            long id = -1;
            _applicationService.Setup(a => a.GetAsync(id)).ReturnsAsync(() => null);
            var sut = new ApplicationController(_applicationService.Object);

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
            var addFormModel = ApplicationMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(a => a.IsTitleDuplicateAsync(addFormModel.Title)).ReturnsAsync(false);
            _applicationService.Setup(a => a.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _applicationService.Setup(a => a.GetAsync(addedId)).ReturnsAsync(ApplicationMockDatas.ApplicationSingleViewModel(addFormModel));

            var sut = new ApplicationController(_applicationService.Object);


            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as CreatedAtActionResult;

            //Assert
            _applicationService.Verify(ap => ap.AddAsync(addFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async void Post_Should_Return_Added_Application()
        {
            //Arrang
            var addFormModel = ApplicationMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(a => a.IsTitleDuplicateAsync(addFormModel.Title)).ReturnsAsync(false);
            _applicationService.Setup(a => a.GetAsync(addedId)).ReturnsAsync(ApplicationMockDatas.ApplicationSingleViewModel(addFormModel));

            _applicationService.Setup(a => a.AddAsync(addFormModel)).ReturnsAsync(addedId);
            var sut = new ApplicationController(_applicationService.Object);


            //Act
            var actionResult = (await sut.Post(addFormModel)) as CreatedAtActionResult;
            var result = actionResult.Value as ApplicationViewModel;

            //Assert            
            _applicationService.Verify(a => a.AddAsync(addFormModel), Times.Once);

            actionResult.Value.Should().NotBeNull();

            result.Id.Should().NotBe(0);
            result.Title.Should().Be(addFormModel.Title);
            result.Description.Should().Be(addFormModel.Description);
        }

        [Fact]
        public async void Post_Duplicate_Title_Return_Status_400()
        {
            //Arrange
            var addFormModel = ApplicationMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(a => a.IsTitleDuplicateAsync(addFormModel.Title)).ReturnsAsync(true);
            _applicationService.Setup(a => a.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _applicationService.Setup(a => a.GetAsync(addedId)).ReturnsAsync(ApplicationMockDatas.ApplicationSingleViewModel(addFormModel));

            var sut = new ApplicationController(_applicationService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _applicationService.Verify(ap => ap.AddAsync(addFormModel), Times.Never);
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
            var updateFormModel = ApplicationMockDatas.UpdateFormModel();

            _applicationService.Setup(a => a.IsExistAsync(updateFormModel.Id)).ReturnsAsync(true);
            _applicationService.Setup(a => a.UpdateAsync(updateFormModel));

            var sut = new ApplicationController(_applicationService.Object);


            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as NoContentResult;

            //Assert
            _applicationService.Verify(ap => ap.UpdateAsync(updateFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void Put_Invalid_Application_Should_Return_Status_400()
        {
            //Arrange
            var updateFormModel = ApplicationMockDatas.UpdateFormModel();

            _applicationService.Setup(a => a.IsExistAsync(updateFormModel.Id)).ReturnsAsync(false);
            _applicationService.Setup(a => a.UpdateAsync(updateFormModel));

            var sut = new ApplicationController(_applicationService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _applicationService.Verify(ap => ap.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_Duplicate_Title_Should_Return_Status_400()
        {
            //Arrange
            var updateFormModel = ApplicationMockDatas.UpdateFormModel();

            _applicationService.Setup(a => a.IsExistAsync(updateFormModel.Id)).ReturnsAsync(true);
            _applicationService.Setup(a => a.IsTitleDuplicateAsync(updateFormModel.Id, updateFormModel.Title)).ReturnsAsync(true);

            _applicationService.Setup(a => a.UpdateAsync(updateFormModel));
            var sut = new ApplicationController(_applicationService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _applicationService.Verify(ap => ap.UpdateAsync(updateFormModel), Times.Never);
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
            _applicationService.Setup(a => a.IsExistAsync(id)).ReturnsAsync(true);
            _applicationService.Setup(a => a.DeleteAsync(id));

            var sut = new ApplicationController(_applicationService.Object);

            //Act
            var actionResult = await sut.Delete(id);
            var result = actionResult as NoContentResult;

            //Assert
            _applicationService.Verify(ap => ap.DeleteAsync(id), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void Delete_Invalid_Application_Return_Status_400()
        {
            //Arrange
            long id = 1;
            _applicationService.Setup(a => a.IsExistAsync(id)).ReturnsAsync(false);

            _applicationService.Setup(a => a.DeleteAsync(id));
            var sut = new ApplicationController(_applicationService.Object);

            //Act
            var actionResult = await sut.Delete(id);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _applicationService.Verify(ap => ap.DeleteAsync(id), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        #endregion
    }
}
