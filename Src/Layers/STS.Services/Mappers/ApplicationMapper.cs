using STS.DataAccessLayer.Entities;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Mappers
{
    public static class ApplicationMapper
    {
        public static IQueryable<ApplicationViewModel> ToViewModel(this IQueryable<Application> query)
        {
            return query.Select(item => new ApplicationViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                CreatedDate = item.CreatedDate,
                ExpirationDuration = item.ExpirationDuration,
                RoleCount = item.Roles.Count(),
                PermissionCount = item.Permissions.Count()
            });
        }

        public static IEnumerable<ApplicationViewModel> ToViewModel(this IEnumerable<Application> query)
        {
            return query.Select(item => new ApplicationViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                CreatedDate = item.CreatedDate,
                ExpirationDuration = item.ExpirationDuration,
                RoleCount = item.Roles.Count(),
                PermissionCount = item.Permissions.Count()
            });
        }

        public static ApplicationViewModel ToViewModel(this Application item)
        {
            return new ApplicationViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                CreatedDate = item.CreatedDate,
                ExpirationDuration = item.ExpirationDuration,
                RoleCount = item.Roles.Count(),
                PermissionCount = item.Permissions.Count()
            };
        }

        public static IQueryable<SelectItemListModel> ToSelectItemList(this IQueryable<Application> query)
        {
            return query.Select(item => new SelectItemListModel
            {
                Text = item.Title,
                Value = item.Id
            });
        }
    }
}
