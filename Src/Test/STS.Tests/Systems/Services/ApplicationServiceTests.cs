using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.CommonModels;
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
    public class ApplicationServiceTests
    {
        private Mock<DbSet<Application>> _mockApplicationSet;
        private Mock<STSDbContext> _mockContext;

        public ApplicationServiceTests()
        {
            _mockApplicationSet = new Mock<DbSet<Application>>();
            _mockContext = new Mock<STSDbContext>();
        }

        #region Add

        [Fact]
        public async void Add_Save_A_Application_Via_Context()
        {
            //Arrange
            var addFormModel = ApplicationMockDatas.AddFormModel();
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.AddAsync(addFormModel);

            //Assert
            _mockApplicationSet.Verify(m => m.AddAsync(It.IsAny<Application>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Get

        [Fact]
        public async void Get_By_Pagination_Should_Return_PagedList_Collection_Application()
        {
            //Arrange
            var pagination = new PaginationParam { PageNumber = 1, PageSize = 10 };
            var data = ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable();

            _mockApplicationSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.GetAsync(pagination);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PagedList<ApplicationViewModel>>();
            result.Items.Should().HaveCount(data.Count());
        }

        [Fact]
        public async void Get_By_Id_Should_Return_Application()
        {
            //Arrange            
            long id = 1;
            var data = ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable();

            _mockApplicationSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.GetAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ApplicationViewModel>();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async void Get_By_Invalid_Id_Should_Return_Null()
        {
            //Arrange            
            long id = -1;
            var data = ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable();

            _mockApplicationSet.IqueryableRegisteration(data);
            _mockContext.Setup(c => c.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.GetAsync(id);

            //Assert
            result.Should().BeNull();
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_Save_A_Application_Via_Context()
        {
            //Arrange
            var updateFormModel = ApplicationMockDatas.UpdateFormModel();
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);
            _mockApplicationSet.Setup(m => m.FindAsync(updateFormModel.Id)).ReturnsAsync(ApplicationMockDatas.EntityModel());

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            await sut.UpdateAsync(updateFormModel);

            //Assert
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Delete

        [Fact]
        public async void Delete_Save_A_Application_Via_Context()
        {
            //Arrange
            long id = 1;
            var entity = ApplicationMockDatas.EntityModel();

            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);
            _mockApplicationSet.Setup(m => m.FindAsync(id)).ReturnsAsync(entity);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            await sut.DeleteAsync(id);

            //Assert
            _mockApplicationSet.Verify(m => m.Remove(entity), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region IsExist

        [Fact]
        public async void IsExist_By_Valid_Id_Shold_Return_True()
        {
            //Arrange
            long id = 1;

            _mockApplicationSet.IqueryableRegisteration(ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.IsExistAsync(id);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsExist_By_Invalid_Id_Shold_Return_False()
        {
            //Arrange
            long id = -1;

            _mockApplicationSet.IqueryableRegisteration(ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
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
            string title = "App_Unique";

            _mockApplicationSet.IqueryableRegisteration(ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(title);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsTitleDuplicate_By_Duplicate_Title_Shold_Return_True()
        {
            //Arrange
            string title = "App_1";

            _mockApplicationSet.IqueryableRegisteration(ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(title);

            //Assert
            result.Should().BeTrue();
        }

        #endregion

        #region IsTitleDuplicate

        [Fact]
        public async void IsTitleDuplicate_By_Id_And_NonDuplicate_Title_Shold_Return_False()
        {
            //Arrange
            long id = 1;
            string title = "App_Unique";

            _mockApplicationSet.IqueryableRegisteration(ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(id, title);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async void IsCaptionDuplicate_By_RoleId_And_Duplicate_Caption_Shold_Return_True()
        {
            //Arrange
            long id = 2;
            string title = "App_1";

            _mockApplicationSet.IqueryableRegisteration(ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(id, title);

            //Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async void IsTitleDuplicate_Exact_Record_Shold_Return_False()
        {
            //Arrange
            long id = 1;
            string title = "App_1";

            _mockApplicationSet.IqueryableRegisteration(ApplicationMockDatas.ApplicationCollectionEntityModels().AsQueryable());
            _mockContext.Setup(m => m.Applications).Returns(_mockApplicationSet.Object);

            //Act
            var sut = new ApplicationService(_mockContext.Object);
            var result = await sut.IsTitleDuplicateAsync(id, title);

            //Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
