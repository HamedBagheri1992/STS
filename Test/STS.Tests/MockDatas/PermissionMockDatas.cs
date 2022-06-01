using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Tests.MockDatas
{
    public static class PermissionMockDatas
    {
        public static List<PermissionViewModel> PermissionCollectionViewModels()
        {
            return new List<PermissionViewModel>
            {
                new PermissionViewModel
                {
                    Id = 1,
                    ApplicationId= 1,
                    Title = "Permission_1",
                    DisplayTitle = "مجوز _ 1"
                },
                new PermissionViewModel
                {
                    Id = 2,
                    ApplicationId= 1,
                    Title = "Permission_2",
                    DisplayTitle = "مجوز _ 2"
                },
                new PermissionViewModel
                {
                    Id = 3,
                    ApplicationId= 1,
                    Title = "Permission_3",
                    DisplayTitle = "مجوز _ 3"
                }
            };
        }

        public static List<Permission> PermissionCollectionEntityModels()
        {
            var role = new Role { Id = 1, Caption = "Role" };
            return new List<Permission>
            {
                new Permission
                {
                    Id = 1,
                    ApplicationId= 1,
                    Title = "Permission_1",
                    DisplayTitle = "مجوز _ 1"
                },
                new Permission
                {
                    Id = 2,
                    ApplicationId= 1,
                    Title = "Permission_2",
                    DisplayTitle = "مجوز _ 2"
                },
                new Permission
                {
                    Id = 3,
                    ApplicationId= 1,
                    Title = "Permission_3",
                    DisplayTitle = "مجوز _ 3"
                }
            };
        }

        public static PermissionViewModel PermissionSingleViewModel()
        {
            return new PermissionViewModel
            {
                Id = 1,
                Title = "Permission_1",
                DisplayTitle = "مجوز _ 1"
            };
        }
        public static PermissionViewModel PermissionSingleViewModel(AddPermissionFormModel formModel)
        {
            return new PermissionViewModel
            {
                Id = 1,
                ApplicationId = formModel.ApplicationId,
                Title = formModel.Title,
                DisplayTitle = formModel.DisplayTitle
            };
        }

        public static AddPermissionFormModel AddFormModel()
        {
            return new AddPermissionFormModel { Title = "Permission_1", DisplayTitle = "مجوز_1", ApplicationId = 1 };
        }

        public static UpdatePermissionFormModel UpdateFormModel()
        {
            return new UpdatePermissionFormModel { Id = 1, Title = "Permission_1", DisplayTitle = "مجوز_1", ApplicationId = 1 };
        }

        public static Permission EntityModel()
        {
            return new Permission { Id = 1, Title = "Permission_1", DisplayTitle = "مجوز_1" };
        }
    }
}
