using STS.DataAccessLayer.Entities;
using STS.DTOs.BaseModels;
using STS.DTOs.CategoryModels.ViewModels;

namespace STS.Services.Mappers
{
    public static class CategoryMapper
    {
        public static IQueryable<CategoryViewModel> ToViewModel(this IQueryable<Category> query)
        {
            return query.Select(item => new CategoryViewModel
            {
                Id = item.Id,
                ApplicationId = item.ApplicationId,
                Title = item.Title,
                ApplicationTitle = item.Application != null ? item.Application.Title : string.Empty
            });
        }

        public static CategoryViewModel ToViewModel(this Category item)
        {
            return new CategoryViewModel
            {
                Id = item.Id,
                ApplicationId = item.ApplicationId,
                Title = item.Title,
                ApplicationTitle = item.Application != null ? item.Application.Title : string.Empty
            };
        }

        public static IQueryable<SelectItemListModel> ToSelectItemList(this IQueryable<Category> query)
        {
            return query.Select(item => new SelectItemListModel
            {
                Text = item.Title,
                Value = item.Id
            });
        }
    }
}
