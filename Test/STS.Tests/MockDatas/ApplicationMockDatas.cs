using STS.DataAccessLayer.Entities;
using STS.DTOs.ApplicationModels.FormModels;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STS.DTOs.BaseModels;

namespace STS.Tests.MockDatas
{
    public static class ApplicationMockDatas
    {
        public static PaginatedResult<ApplicationViewModel> ApplicationPagedCollectionViewModels(PaginationParam pagination)
        {
            var Items = ApplicationCollectionViewModels();
            return new PaginatedResult<ApplicationViewModel>(Items, 3, pagination.PageNumber, pagination.PageSize);
        }

        public static List<ApplicationViewModel> ApplicationCollectionViewModels()
        {
            return new List<ApplicationViewModel>
            {
                new ApplicationViewModel
                {
                    Id = 1,
                    Title = "App_1",
                    Description = string.Empty,
                    CreatedDate = DateTime.Now,
                    RoleCount= 0,
                    PermissionCount = 0
                },
                new ApplicationViewModel
                {
                   Id = 2,
                   Title = "App_2",
                   Description = string.Empty,
                   CreatedDate = DateTime.Now,
                   RoleCount= 0,
                   PermissionCount = 0
                },
                new ApplicationViewModel
                {
                    Id = 3,
                    Title = "App_3",
                    Description = string.Empty,
                    CreatedDate = DateTime.Now,
                    RoleCount= 0,
                    PermissionCount = 0
                }
            };
        }

        public static List<Application> ApplicationCollectionEntityModels()
        {
            return new List<Application>
            {
                new Application
                {
                    Id = 1,
                    Title = "App_1",
                    Description = string.Empty,
                    CreatedDate = DateTime.Now,
                    SecretKey = Guid.NewGuid()
                },
                new Application
                {
                     Id = 2,
                    Title = "App_2",
                    Description = string.Empty,
                    CreatedDate = DateTime.Now,
                    SecretKey = Guid.NewGuid()
                },
                new Application
                {
                    Id = 3,
                    Title = "App_3",
                    Description = string.Empty,
                    CreatedDate = DateTime.Now,
                    SecretKey = Guid.NewGuid()
                }
            };
        }

        public static ApplicationViewModel ApplicationSingleViewModel()
        {
            return new ApplicationViewModel
            {
                Id = 1,
                Title = "App_1",
                Description = string.Empty,
                CreatedDate = DateTime.Now,
                RoleCount = 0,
                PermissionCount = 0
            };
        }
        public static ApplicationViewModel ApplicationSingleViewModel(AddApplicationFormModel formModel)
        {
            return new ApplicationViewModel
            {
                Id = 1,
                Title = formModel.Title,
                Description = formModel.Description,
                CreatedDate = DateTime.Now
            };
        }

        public static AddApplicationFormModel AddFormModel()
        {
            return new AddApplicationFormModel { Title = "App_1", Description = "Desc_1" };
        }

        public static List<SelectItemListModel> SelectItemListModels()
        {
            return new List<SelectItemListModel> { new SelectItemListModel { Value = 1, Text = "App1" }, new SelectItemListModel { Value = 2, Text = "App2" } };
        }

        public static UpdateApplicationFormModel UpdateFormModel()
        {
            return new UpdateApplicationFormModel { Id = 1, Title = "App_1", Description = "Desc_1" };
        }

        public static Application EntityModel()
        {
            return new Application { Id = 1, Title = "App_1", Description = "Desc_1", SecretKey = Guid.NewGuid(), CreatedDate = DateTime.Now };
        }
    }
}
