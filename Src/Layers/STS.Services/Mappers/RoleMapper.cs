using STS.Common.BaseModels;
using STS.DataAccessLayer.Entities;
using STS.DTOs.RoleModels.FormModels;
using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Mappers
{
    public static class RoleMapper
    {
        public static IQueryable<RoleViewModel> ToViewModel(this IQueryable<Role> query)
        {
            return query.Select(item => new RoleViewModel
            {
                Id = item.Id,
                Caption = item.Caption,
                ApplicationTitle = item.Application.Title,
                ApplicationId = item.Application.Id,
                Permissions = item.Permissions.ToViewModel()
            });
        }

        public static IEnumerable<RoleViewModel> ToViewModel(this IEnumerable<Role> query)
        {
            return query.Select(item => new RoleViewModel
            {
                Id = item.Id,
                Caption = item.Caption,
                ApplicationTitle = item.Application.Title,
                ApplicationId = item.Application.Id,
                Permissions = item.Permissions.ToViewModel()
            });
        }

        public static RoleViewModel ToViewModel(this Role item)
        {
            return new RoleViewModel
            {
                Id = item.Id,
                Caption = item.Caption,
                ApplicationTitle = item.Application.Title,
                ApplicationId = item.Application.Id,
                Permissions = item.Permissions.ToViewModel()
            };
        }

        public static void GetConsistency(this Role role, UpdateRolePermissionFormModel updateRolePermissionFormModel, out List<Permission> deletePermissions, out List<long> addPermissionId)
        {
            deletePermissions = role.Permissions.Where(p => updateRolePermissionFormModel.PermissionIds.All(u => u != p.Id)).ToList();
            addPermissionId = updateRolePermissionFormModel.PermissionIds.Where(u => role.Permissions.All(p => p.Id != u)).ToList();
        }
    }
}
