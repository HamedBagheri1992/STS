using STS.Common.BaseModels;
using STS.DataAccessLayer.Entities;
using STS.DTOs.BaseModels;
using STS.DTOs.UserModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Mappers
{
    public static class UserMapper
    {
        public static IQueryable<UserViewModel> ToViewModel(this IQueryable<User> query)
        {
            return query.Select(item => new UserViewModel
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.UserName,
                FullName = item.FullName,
                IsActive = item.IsActive,
                ExpirationDate = item.ExpirationDate,
                LastLogin = item.LastLogin,
                ModifiedDate = item.ModifiedDate,
                CreatedDate = item.CreatedDate,
                Applications = item.Applications.ToViewModel(),
                Roles = item.Roles.ToViewModel(),
                OrganizationIds = item.Organizations.Select(or => or.Id)
            });
        }

        public static UserViewModel ToViewModel(this User item)
        {
            return new UserViewModel
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.UserName,
                FullName = item.FullName,
                IsActive = item.IsActive,
                ExpirationDate = item.ExpirationDate,
                LastLogin = item.LastLogin,
                ModifiedDate = item.ModifiedDate,
                CreatedDate = item.CreatedDate,
                Applications = item.Applications.ToViewModel(),
                Roles = item.Roles.ToViewModel(),
                OrganizationIds = item.Organizations.Select(or => or.Id)
            };
        }

        public static UserIdentityBaseModel ToUserIdentityBaseModel(this User user, Application application, List<KeyValueModel> rolePairs, List<KeyValueModel> permissionPairs)
        {
            return new UserIdentityBaseModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                IsActive = user.IsActive,
                ApplicationId = application.Id,
                ExpirationDate = user.ExpirationDate,
                ExpirationDuration = application.ExpirationDuration,
                RolePairs = rolePairs,
                PermissionPairs = permissionPairs
            };
        }

        public static IEnumerable<KeyValueModel> ToRoleKeyValueModel(this User user)
        {
            return user.Roles.Select(r => new KeyValueModel { Key = r.Id, Value = r.Caption });
        }


        public static IEnumerable<KeyValueModel> ToPermissionKeyValueModel(this User user)
        {
            return user.Roles.SelectMany(r => r.Permissions).GroupBy(p => p.Id).Select(p => new KeyValueModel
            {
                Key = p.Key,
                Value = p.First().Title
            });
        }
    }
}
