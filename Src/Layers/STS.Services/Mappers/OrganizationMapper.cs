using STS.DataAccessLayer.Entities;
using STS.DTOs.OrganizationModel.ViewModels;

namespace STS.Services.Mappers
{
    public static class OrganizationMapper
    {
        public static IQueryable<OrganizationViewModel> ToViewModel(this IQueryable<Organization> query)
        {
            return query.Select(item => new OrganizationViewModel
            {
                Id = item.Id,
                ParentId = item.ParentId,
                Title = item.Title,
                Tag = item.Tag,
                Description = item.Description
            });
        }

        public static OrganizationViewModel ToViewModel(this Organization item)
        {
            return new OrganizationViewModel
            {
                Id = item.Id,
                ParentId = item.ParentId,
                Title = item.Title,
                Tag = item.Tag,
                Description = item.Description
            };
        }


        public static List<OrganizationViewModel> ToManageOrganizations(this List<OrganizationViewModel> organizations, long orgId)
        {
            var orgs = new List<OrganizationViewModel>();
            organizations.Where(or => or.Id == orgId).ToList().ForEach(or =>
             {
                 var org = new OrganizationViewModel();
                 org.Id = or.Id;
                 org.ParentId = or.ParentId;
                 org.Title = or.Title;
                 org.Tag = or.Tag;
                 org.Description = or.Description;

                 orgs.Add(org);
                 FillChild(organizations, orgs, or.Id);
             });
            return orgs;
        }

        private static void FillChild(List<OrganizationViewModel> organizations, List<OrganizationViewModel> orgs, long Id)
        {
            if (organizations.Count > 0)
            {
                organizations.Where(or => or.ParentId == Id).ToList().ForEach(or =>
                 {
                     var childNode = new OrganizationViewModel
                     {
                         Id = or.Id,
                         ParentId = or.ParentId,
                         Title = or.Title,
                         Tag = or.Tag,
                         Description = or.Description
                     };

                     orgs.Add(childNode);

                     FillChild(organizations, orgs, or.Id);
                 });
            }
        }
    }
}
