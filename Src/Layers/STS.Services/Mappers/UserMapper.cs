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
                IsDeleted = item.IsDeleted,
                LastLogin = item.LastLogin,
                ModifiedDate = item.ModifiedDate,
                CreatedDate = item.CreatedDate,
                Applications = item.Applications.ToViewModel(),
                Roles = item.Roles.ToViewModel()
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
                IsDeleted = item.IsDeleted,
                LastLogin = item.LastLogin,
                ModifiedDate = item.ModifiedDate,
                CreatedDate = item.CreatedDate,
                Applications = item.Applications.ToViewModel(),
                Roles = item.Roles.ToViewModel()
            };
        }

        public static UserIdentityBaseModel ToBaseModel(this User item)
        {
            return new UserIdentityBaseModel
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName,
                UserName = item.UserName,
                FullName = item.FullName,
                Roles = item.Roles.ToViewModel()
            };
        }
    }
}
