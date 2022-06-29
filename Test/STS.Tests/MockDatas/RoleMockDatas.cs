using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.DTOs.RoleModels.FormModels;
using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Tests.MockDatas
{
    public static class RoleMockDatas
    {
        public static PaginatedResult<RoleViewModel> RolePagedCollectionViewModels(PaginationParam pagination)
        {
            var Items = new List<RoleViewModel>
            {
                new RoleViewModel
                {
                    Id = 1,
                    Caption = "Role_1",
                    ApplicationId = 1,
                    ApplicationTitle ="App_1",
                    Permissions = new List<PermissionViewModel>()
                },
                new RoleViewModel
                {
                     Id = 2,
                    Caption = "Role_2",
                    ApplicationId = 1,
                    ApplicationTitle ="App_1",
                    Permissions = new List<PermissionViewModel>()
                },
                new RoleViewModel
                {
                    Id = 3,
                    Caption = "Role_3",
                    ApplicationId = 1,
                    ApplicationTitle ="App_1",
                    Permissions = new List<PermissionViewModel>()
                }
            };

            return new PaginatedResult<RoleViewModel>(Items, 3, pagination.PageNumber, pagination.PageSize);
        }

        public static List<RoleViewModel> RoleCollectionViewModels()
        {
            return new List<RoleViewModel>
            {
                new RoleViewModel
                {
                    Id = 1,
                    Caption = "Role_1",
                    ApplicationId = 1,
                    ApplicationTitle ="App_1",
                    Permissions = new List<PermissionViewModel>()
                },
                new RoleViewModel
                {
                     Id = 2,
                    Caption = "Role_2",
                    ApplicationId = 1,
                    ApplicationTitle ="App_1",
                    Permissions = new List<PermissionViewModel>()
                },
                new RoleViewModel
                {
                    Id = 3,
                    Caption = "Role_3",
                    ApplicationId = 1,
                    ApplicationTitle ="App_1",
                    Permissions = new List<PermissionViewModel>()
                }
            };
        }

        public static List<Role> RoleCollectionEntityModels()
        {
            var application = new Application { Id = 1, Title = "Application", Description = "Description", SecretKey = Guid.NewGuid(), CreatedDate = DateTime.Now };
            return new List<Role>
            {
                new Role
                {
                    Id = 1,
                    Caption = "Role_1",
                    ApplicationId = 1,
                    Application = application
                },
                new Role
                {
                    Id = 2,
                    Caption = "Role_2",
                    ApplicationId = 1,
                    Application = application
                },
                new Role
                {
                    Id = 3,
                    Caption = "Role_3",
                    ApplicationId = 1,
                    Application = application
                }
            };
        }

        public static RoleViewModel RoleSingleViewModel()
        {
            return new RoleViewModel
            {
                Id = 1,
                Caption = "Role_1",
                ApplicationId = 1,
                ApplicationTitle = "App_1",
                Permissions = new List<PermissionViewModel>()
            };
        }

        public static UpdateRolePermissionFormModel GetUpdateRolePermissionFormModel()
        {
            return new UpdateRolePermissionFormModel { RoleId = 1, PermissionIds = new List<long> { 1, 2, 3, 4 } };
        }
        public static RoleViewModel RoleSingleViewModel(AddRoleFormModel formModel)
        {
            return new RoleViewModel
            {
                Id = 1,
                Caption = formModel.Caption,
                ApplicationId = formModel.ApplicationId,
                ApplicationTitle = $"App_{formModel.ApplicationId}",
                Permissions = new List<PermissionViewModel>()
            };
        }

        public static AddRoleFormModel AddFormModel()
        {
            return new AddRoleFormModel { Caption = "Role_1", ApplicationId = 1 };
        }

        public static UpdateRoleFormModel UpdateFormModel()
        {
            return new UpdateRoleFormModel { Id = 1, Caption = "Role_1", ApplicationId = 1 };
        }

        public static Role EntityModel()
        {
            return new Role { Id = 1, Caption = "Role_1", ApplicationId = 1 };
        }
    }
}
