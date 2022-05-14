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
        public static IEnumerable<PermissionViewModel> PermissionCollectionViewModels()
        {
            return new List<PermissionViewModel>
            {
                new PermissionViewModel
                {
                    Id = 1,
                    Title = "Permission_1",
                    DisplayTitle = "مجوز _ 1"
                },
                new PermissionViewModel
                {
                    Id = 2,
                    Title = "Permission_2",
                    DisplayTitle = "مجوز _ 2"
                },
                new PermissionViewModel
                {
                    Id = 3,
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
                Title = formModel.Title,
                DisplayTitle = formModel.DisplayTitle,
                RoleId = formModel.RoleId
            };
        }
    }
}
