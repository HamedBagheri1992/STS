
using STS.DTOs.OrganizationModel.FormModels;
using STS.DTOs.OrganizationModel.ViewModels;
using System.Collections.Generic;

namespace STS.Tests.MockDatas
{
    public static class OrganizationMockDatas
    {
        public static List<OrganizationViewModel> OrganizationCollectionViewModels()
        {
            return new List<OrganizationViewModel>
            {
                new OrganizationViewModel
                {
                    Id = 1,
                    Title = "Org_1",
                    Description = string.Empty,
                    ParentId = null,
                    Tag = string.Empty
                },
                new OrganizationViewModel
                {
                   Id = 2,
                   Title = "Org_2",
                    Description = string.Empty,
                    ParentId = 1,
                    Tag = string.Empty
                },
                new OrganizationViewModel
                {
                    Id = 3,
                    Title = "Org_3",
                    Description = string.Empty,
                    ParentId = 2,
                    Tag = string.Empty
                }
            };
        }

        public static OrganizationViewModel OrganizationSingleViewModels(long id)
        {
            return new OrganizationViewModel { Id = id, Title = "Org", ParentId = null, Description = string.Empty, Tag = string.Empty };
        }

        public static AddOrganizationFormModel GetAddFormModel(long? parentId)
        {
            return new AddOrganizationFormModel { ParentId = parentId, Title = "Org_Add", Description = "Desc", Tag = "Tag" };
        }
    }
}
