using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.BaseModels;
using STS.DTOs.CategoryModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Services.Impls;
using STS.Tests.Helpers;
using STS.Tests.MockDatas;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace STS.Tests.Systems.Services
{
    public class CategoryServiceTests
    {
        private Mock<DbSet<Category>> _mockCategorySet;
        private Mock<STSDbContext> _mockContext;

        public CategoryServiceTests()
        {
            _mockCategorySet = new Mock<DbSet<Category>>();
            _mockContext = new Mock<STSDbContext>();
        }


        #region Add

        [Fact]
        public async void Add_Save_A_Application_Via_Context()
        {
            //Arrange
            var addFormModel = CategoryMockDatas.AddFormModel();
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.AddAsync(addFormModel);

            //Assert
            _mockCategorySet.Verify(m => m.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_Save_A_Application_Via_Context()
        {
            //Arrange
            var updateFormModel = CategoryMockDatas.UpdateFormModel();
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);
            _mockCategorySet.Setup(m => m.FindAsync(updateFormModel.Id)).ReturnsAsync(CategoryMockDatas.EntityModel());

            //Act
            var sut = new CategoryService(_mockContext.Object);
            await sut.UpdateAsync(updateFormModel);

            //Assert
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Get

        [Fact]
        public async void Get_By_Pagination_Should_Return_PagedList_Collection_Category()
        {
            //Arrange
            var pagination = new PaginationParam { PageNumber = 1, PageSize = 10 };
            var data = CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable();

            _mockCategorySet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.GetAsync(pagination);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PaginatedResult<CategoryViewModel>>();
            result.Items.Should().HaveCount(data.Count());
        }

        [Fact]
        public async void Get_By_Id_Should_Return_Category()
        {
            //Arrange            
            long id = 1;
            var data = CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable();

            _mockCategorySet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.GetAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryViewModel>();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async void Get_By_Invalid_Id_Should_Return_Null()
        {
            //Arrange            
            long id = -1;
            var data = CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable();

            _mockCategorySet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.GetAsync(id);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async void GetItemList_Should_Return_SelectItem_Collection()
        {
            //Arrang
            long applicationId = 1;
            var data = CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable();

            _mockCategorySet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.GetItemListAsync(applicationId);

            //Assert
            result.Should().BeOfType(typeof(List<SelectItemListModel>));
            result.Should().HaveCount(data.Count());
        }

        #endregion

        #region IsExist

        [Fact]
        public async void IsExist_By_Valid_Id_Shold_Return_True()
        {
            //Arrange
            long id = 1;

            _mockCategorySet.IqueryableRegisteration(CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.IsExistAsync(id);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsExist_By_Invalid_Id_Shold_Return_False()
        {
            //Arrange
            long id = -1;

            _mockCategorySet.IqueryableRegisteration(CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.IsExistAsync(id);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region IsTitleDuplicate

        [Fact]
        public async void IsTitleDuplicate_By_NonDuplicate_Title_Shold_Return_False()
        {
            //Arrange
            string title = "Cat_Unique";
            long applicationId = 1;

            _mockCategorySet.IqueryableRegisteration(CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(title, applicationId);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsTitleDuplicate_By_Duplicate_Title_Shold_Return_True()
        {
            //Arrange
            string title = "title1";
            long applicationId = 1;

            _mockCategorySet.IqueryableRegisteration(CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(title, applicationId);

            //Assert
            result.Should().BeTrue();
        }

        #endregion

        #region IsTitleDuplicate

        [Fact]
        public async void IsTitleDuplicate_By_Id_And_NonDuplicate_Title_Shold_Return_False()
        {
            //Arrange
            long applicationId = 1;
            string title = "Cat_Unique";

            _mockCategorySet.IqueryableRegisteration(CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(title, applicationId);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsTitleDuplicate_By_ApplicationId_And_Duplicate_Title_Shold_Return_True()
        {
            //Arrange
            long applicationId = 1;
            string title = "title1";

            _mockCategorySet.IqueryableRegisteration(CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(title, applicationId);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void IsTitleDuplicate_Exact_Record_Shold_Return_False()
        {
            //Arrange
            long id = 1;
            long applicationId = 1;
            string title = "title1";

            _mockCategorySet.IqueryableRegisteration(CategoryMockDatas.CategoryCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Categories).Returns(_mockCategorySet.Object);

            //Act
            var sut = new CategoryService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(id, title, applicationId);

            //Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
