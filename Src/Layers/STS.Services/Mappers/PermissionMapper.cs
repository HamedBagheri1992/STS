using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.ViewModels;

namespace STS.Services.Mappers
{
    public static class PermissionMapper
    {
        public static IQueryable<PermissionViewModel> ToViewModel(this IQueryable<Permission> query)
        {
            return query.Select(item => new PermissionViewModel
            {
                Id = item.Id,
                ApplicationId = item.ApplicationId,
                DisplayTitle = item.DisplayTitle,
                Title = item.Title,
                CategoryId = item.CategoryId,
                CategoryTitle = item.CategoryId.HasValue ? item.Category.Title : string.Empty
            });
        }

        public static IEnumerable<PermissionViewModel> ToViewModel(this IEnumerable<Permission> query)
        {
            return query.Select(item => new PermissionViewModel
            {
                Id = item.Id,
                ApplicationId = item.ApplicationId,
                DisplayTitle = item.DisplayTitle,
                Title = item.Title,
                CategoryId = item.CategoryId,
                CategoryTitle = item.CategoryId.HasValue ? item.Category.Title : string.Empty
            });
        }

        public static PermissionViewModel ToViewModel(this Permission item)
        {
            return new PermissionViewModel
            {
                Id = item.Id,
                ApplicationId = item.ApplicationId,
                DisplayTitle = item.DisplayTitle,
                Title = item.Title,
                CategoryId = item.CategoryId,
                CategoryTitle = item.CategoryId.HasValue ? item.Category.Title : string.Empty
            };
        }
    }
}
