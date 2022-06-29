
using STS.DataAccessLayer.Entities;
using STS.DTOs.BaseModels;
using STS.DTOs.CategoryModels.FormModels;
using STS.DTOs.CategoryModels.ViewModels;
using STS.DTOs.ResultModels;
using System;
using System.Collections.Generic;

namespace STS.Tests.MockDatas
{
    public static class CategoryMockDatas
    {
        public static PaginatedResult<CategoryViewModel> CategoryPagedCollectionViewModels(PaginationParam pagination)
        {
            var Items = new List<CategoryViewModel>
            {
                new CategoryViewModel { Id = 1, ApplicationId = 1 , Title = "Category_1" },
                new CategoryViewModel { Id = 2, ApplicationId = 1 , Title = "Category_2" },
                new CategoryViewModel { Id = 3, ApplicationId = 1 , Title = "Category_3" }
            };

            return new PaginatedResult<CategoryViewModel>(Items, 3, pagination.PageNumber, pagination.PageSize);
        }

        public static CategoryViewModel CategorySingleViewModel()
        {
            return new CategoryViewModel { Id = 1, ApplicationId = 1, Title = "Category_1" };
        }

        public static AddCategoryFormModel AddFormModel()
        {
            return new AddCategoryFormModel { ApplicationId = 1, Title = "Category_1" };
        }

        internal static CategoryViewModel CategorySingleViewModel(AddCategoryFormModel addFormModel)
        {
            return new CategoryViewModel { Id = 1, ApplicationId = addFormModel.ApplicationId, Title = addFormModel.Title };
        }

        public static UpdateCategoryFormModel UpdateFormModel()
        {
            return new UpdateCategoryFormModel { Id = 1, ApplicationId = 1, Title = "Category_1" };
        }

        public static Category EntityModel()
        {
            return new Category { Id = 1, Title = "title", ApplicationId = 1 };
        }

        public static IEnumerable<SelectItemListModel> SelectItemListModels()
        {
            return new List<SelectItemListModel> { new SelectItemListModel { Value = 1, Text = "Cat1" }, new SelectItemListModel { Value = 2, Text = "Cat2" } };
        }

        public static List<Category> CategoryCollectionEntityModels()
        {
            return new List<Category>
            {
                new Category { Id = 1, Title = "title1", ApplicationId = 1},
                new Category { Id = 2, Title = "title2", ApplicationId = 1},
                new Category { Id = 3, Title = "title3", ApplicationId = 1}
            };
        }
    }
}
