using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using STS.Common.BaseModels;
using STS.DTOs.BaseModels;
using STS.DTOs.CategoryModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Interfaces.Contracts;
using STS.Tests.MockDatas;
using STS.WebApi.Controllers.V1;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace STS.Tests.Systems.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<IApplicationService> _applicationService;
        private readonly Mock<ICategoryService> _categoryService;

        public CategoryControllerTests()
        {
            _applicationService = new Mock<IApplicationService>();
            _categoryService = new Mock<ICategoryService>();
        }

        #region Get

        [Fact]
        public async void Get_Should_Return_Status_200()
        {
            //Arrange
            var pagination = new PaginationParam { PageNumber = 1, PageSize = 10 };
            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

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
            var mockCategory = CategoryMockDatas.CategoryPagedCollectionViewModels(pagination);

            _categoryService.Setup(c => c.GetAsync(pagination)).ReturnsAsync(mockCategory);
            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Get(pagination);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var app = result.Value as PaginatedResult<CategoryViewModel>;
            app.Items.Should().NotBeNull();
            app.Items.Should().HaveCount(mockCategory.Items.Count);
        }

        [Fact]
        public async void Get_By_Id_Should_Return_Single_Application()
        {
            //Arrange
            long id = 1;
            _categoryService.Setup(c => c.GetAsync(id)).ReturnsAsync(CategoryMockDatas.CategorySingleViewModel());
            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Get(id);
            var result = actionResult.Result as OkObjectResult;


            //Assert
            result.Should().NotBeNull();
            var app = result.Value as CategoryViewModel;
            app.Id.Should().NotBe(0);
            app.Title.Should().NotBeNull();
        }

        [Fact]
        public async void Get_By_Invalid_Id_Should_Return_Status_404()
        {
            //Arrange
            long id = It.IsAny<long>();
            _categoryService.Setup(c => c.GetAsync(id)).ReturnsAsync(() => null);
            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Get(id);
            var actionStatus = actionResult.Result as NotFoundObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            actionStatus.StatusCode.Should().Be(404);
        }

        #endregion

        #region GetItemList

        [Fact]
        public async void GetItemList_Should_Return_Status_200()
        {
            //Arrang
            long applicationId = 1;
            _applicationService.Setup(c => c.IsExistAsync(applicationId)).ReturnsAsync(true);
            _categoryService.Setup(c => c.GetItemListAsync(applicationId)).ReturnsAsync(CategoryMockDatas.SelectItemListModels());
            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.GetItemList(applicationId);
            var result = actionResult.Result as OkObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<SelectItemListModel>>();
        }

        [Fact]
        public async void GetItemList_By_invalid_ApplicationId_Should_Return_Status_400()
        {
            //Arrang
            long applicationId = 1;
            _applicationService.Setup(c => c.IsExistAsync(applicationId)).ReturnsAsync(false);
            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.GetItemList(applicationId);
            var result = actionResult.Result as BadRequestObjectResult;

            //Assert
            actionResult.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
        }

        #endregion

        #region Post

        [Fact]
        public async void Post_Should_Return_Status_201()
        {
            //Arrang
            var addFormModel = CategoryMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(a => a.IsExistAsync(addFormModel.ApplicationId)).ReturnsAsync(true);
            _categoryService.Setup(c => c.IsTitleDuplicateAsync(addFormModel.Title, addFormModel.ApplicationId)).ReturnsAsync(false);
            _categoryService.Setup(c => c.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _categoryService.Setup(c => c.GetAsync(addedId)).ReturnsAsync(CategoryMockDatas.CategorySingleViewModel(addFormModel));

            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);


            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as CreatedAtActionResult;

            //Assert
            _categoryService.Verify(ca => ca.AddAsync(addFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async void Post_Should_Return_Added_Category()
        {
            //Arrang
            var addFormModel = CategoryMockDatas.AddFormModel();
            var addedId = 1;

            _applicationService.Setup(a => a.IsExistAsync(addFormModel.ApplicationId)).ReturnsAsync(true);
            _categoryService.Setup(c => c.IsTitleDuplicateAsync(addFormModel.Title, addFormModel.ApplicationId)).ReturnsAsync(false);
            _categoryService.Setup(c => c.AddAsync(addFormModel)).ReturnsAsync(addedId);
            _categoryService.Setup(c => c.GetAsync(addedId)).ReturnsAsync(CategoryMockDatas.CategorySingleViewModel(addFormModel));

            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = (await sut.Post(addFormModel)) as CreatedAtActionResult;
            var result = actionResult.Value as CategoryViewModel;

            //Assert            
            actionResult.Value.Should().NotBeNull();

            result.Id.Should().NotBe(0);
            result.Title.Should().Be(addFormModel.Title);
            result.ApplicationId.Should().Be(addFormModel.ApplicationId);
        }

        [Fact]
        public async void Post_By_Invalid_ApplicationId_Return_Status_400()
        {
            //Arrange
            var addFormModel = CategoryMockDatas.AddFormModel();

            _applicationService.Setup(a => a.IsExistAsync(addFormModel.ApplicationId)).ReturnsAsync(false);

            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _categoryService.Verify(ca => ca.AddAsync(addFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Post_Duplicate_Title_Return_Status_400()
        {
            //Arrange
            var addFormModel = CategoryMockDatas.AddFormModel();

            _applicationService.Setup(a => a.IsExistAsync(addFormModel.ApplicationId)).ReturnsAsync(true);
            _categoryService.Setup(c => c.IsTitleDuplicateAsync(addFormModel.Title, addFormModel.ApplicationId)).ReturnsAsync(true);

            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Post(addFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _categoryService.Verify(ca => ca.AddAsync(addFormModel), Times.Never);
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
            var updateFormModel = CategoryMockDatas.UpdateFormModel();

            _applicationService.Setup(a => a.IsExistAsync(updateFormModel.ApplicationId)).ReturnsAsync(true);
            _categoryService.Setup(a => a.IsExistAsync(updateFormModel.Id)).ReturnsAsync(true);
            _categoryService.Setup(c => c.IsTitleDuplicateAsync(updateFormModel.Id, updateFormModel.Title, updateFormModel.ApplicationId)).ReturnsAsync(false);
            _categoryService.Setup(c => c.UpdateAsync(updateFormModel));

            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as NoContentResult;

            //Assert
            _categoryService.Verify(ca => ca.UpdateAsync(updateFormModel), Times.Once);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async void Put_By_Invalid_ApplicationId_Return_Status_400()
        {
            //Arrange
            var updateFormModel = CategoryMockDatas.UpdateFormModel();
            _applicationService.Setup(a => a.IsExistAsync(updateFormModel.ApplicationId)).ReturnsAsync(false);

            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _categoryService.Verify(ca => ca.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_By_Invalid_CategoryId_Return_Status_400()
        {
            //Arrange
            var updateFormModel = CategoryMockDatas.UpdateFormModel();
            _applicationService.Setup(a => a.IsExistAsync(updateFormModel.ApplicationId)).ReturnsAsync(true);
            _categoryService.Setup(a => a.IsExistAsync(updateFormModel.Id)).ReturnsAsync(false);

            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _categoryService.Verify(ca => ca.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }

        [Fact]
        public async void Put_Duplicate_Title_Should_Return_Status_400()
        {
            //Arrange
            var updateFormModel = CategoryMockDatas.UpdateFormModel();

            _applicationService.Setup(a => a.IsExistAsync(updateFormModel.ApplicationId)).ReturnsAsync(false);
            _categoryService.Setup(c => c.IsTitleDuplicateAsync(updateFormModel.Id, updateFormModel.Title, updateFormModel.ApplicationId)).ReturnsAsync(true);
            _categoryService.Setup(c => c.UpdateAsync(updateFormModel));

            var sut = new CategoryController(_categoryService.Object, _applicationService.Object);

            //Act
            var actionResult = await sut.Put(updateFormModel);
            var result = actionResult as BadRequestObjectResult;

            //Assert
            _categoryService.Verify(ca => ca.UpdateAsync(updateFormModel), Times.Never);
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeOfType(typeof(ErrorDetails));
        }


        #endregion
    }
}
