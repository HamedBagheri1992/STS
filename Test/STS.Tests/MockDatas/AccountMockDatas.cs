
using STS.Common.BaseModels;
using STS.Common.Extensions;
using STS.DataAccessLayer.Entities;
using STS.DTOs.AccountModels.FormModels;
using STS.DTOs.AccountModels.ViewModels;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.RoleModels.ViewModels;
using STS.DTOs.UserModels.ViewModels;
using System;
using System.Collections.Generic;

namespace STS.Tests.MockDatas
{
    public static class AccountMockDatas
    {

        public static LoginFormModel ValidLoginFormModel()
        {
            return new LoginFormModel { UserName = "UserName", Password = "123", AppId = 1, SecretKey = Guid.NewGuid() };
        }


        public static LoginFormModel InvalidLoginFormModel()
        {
            return new LoginFormModel { UserName = "User Name", Password = "123", AppId = 1, SecretKey = Guid.NewGuid() };
        }


        public static UserIdentityBaseModel LoginUserIdentityBaseModel()
        {
            return new UserIdentityBaseModel
            {
                Id = 1,
                FirstName = "firstName",
                LastName = "lastName",
                FullName = "firstName lastName",
                ApplicationId = 1,
                UserName = "User1",
                IsActive = true,
                ExpirationDate = DateTime.Now.AddDays(30),
                ExpirationDuration = 60,
                PermissionPairs = new List<KeyValueModel>(),
                RolePairs = new List<KeyValueModel>()
            };
        }

        public static UserIdentityBaseModel LoginDeactivatedUserIdentityBaseModel()
        {
            return new UserIdentityBaseModel
            {
                Id = 1,
                FirstName = "firstName",
                LastName = "lastName",
                FullName = "firstName lastName",
                ApplicationId = 1,
                UserName = "User1",
                IsActive = false,
                ExpirationDate = DateTime.Now.AddDays(30),
                ExpirationDuration = 60,
                PermissionPairs = new List<KeyValueModel>(),
                RolePairs = new List<KeyValueModel>()
            };
        }

        public static UserIdentityBaseModel LoginExpiredUserIdentityBaseModel()
        {
            return new UserIdentityBaseModel
            {
                Id = 1,
                FirstName = "firstName",
                LastName = "lastName",
                FullName = "firstName lastName",
                ApplicationId = 1,
                UserName = "User1",
                IsActive = true,
                ExpirationDate = DateTime.Now.AddDays(-1),
                ExpirationDuration = 60,
                PermissionPairs = new List<KeyValueModel>(),
                RolePairs = new List<KeyValueModel>()
            };
        }

        public static List<User> UserCollectionEntityModelForLogin(Guid secretKey)
        {
            var application = new Application { Id = 1, Title = "app1", SecretKey = secretKey, ExpirationDuration = 60, CreatedDate = DateTime.Now };
            var role = new Role { Id = 1, Caption = "Role1", ApplicationId = 1, Application = application };

            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "F_1",
                    LastName = "L_1",
                    UserName = "U1",
                    IsActive = true,
                    LastLogin = null,
                    CreatedDate = DateTime.Now,
                    ModifiedDate= DateTime.Now,
                    Password = "123".ToHashPassword(),
                    ExpirationDate = DateTime.Now.AddDays(100),
                    Applications = new List<Application> {application },
                    Roles = new List<Role> {role}
                }
            };
        }

        public static UserIdentityBaseModel UserSingleIdentityBaseModel()
        {
            var rolePairs = new List<KeyValueModel>
            {
                new KeyValueModel{Key = 1, Value = "admin"}
            };

            var permissionPairs = new List<KeyValueModel>
            {
                new KeyValueModel{Key = 1, Value = "STS-Permission"}
            };

            return new UserIdentityBaseModel
            {
                Id = 1,
                FirstName = "firstName",
                LastName = "lastName",
                FullName = "firstName lastName",
                ApplicationId = 1,
                UserName = "User1",
                IsActive = true,
                ExpirationDate = DateTime.Now.AddDays(10),
                ExpirationDuration = 60,
                PermissionPairs = permissionPairs,
                RolePairs = rolePairs
            };
        }
    }
}
