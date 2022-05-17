using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Mappers
{
    public static class PermissionMapper
    {
        public static IQueryable<PermissionViewModel> ToViewModel(this IQueryable<Permission> query)
        {
            return query.Select(item => new PermissionViewModel
            {
                Id = item.Id,
                RoleId = item.RoleId,
                DisplayTitle = item.DisplayTitle,
                Title = item.Title
            });
        }

        public static IEnumerable<PermissionViewModel> ToViewModel(this IEnumerable<Permission> query)
        {
            return query.Select(item => new PermissionViewModel
            {
                Id = item.Id,
                RoleId = item.RoleId,
                DisplayTitle = item.DisplayTitle,
                Title = item.Title
            });
        }

        public static PermissionViewModel ToViewModel(this Permission item)
        {
            return new PermissionViewModel
            {
                Id = item.Id,
                RoleId = item.RoleId,
                DisplayTitle = item.DisplayTitle,
                Title = item.Title
            };
        }
    }
}
