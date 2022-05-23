using STS.DataAccessLayer.Entities;
using STS.DTOs.ApplicationModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Mappers
{
    public static class ApplicationMapper
    {
        public static IQueryable<ApplicationViewModel> ToViewModel(this IQueryable<Application> query)
        {
            return query.Select(item => new ApplicationViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                CreatedDate = item.CreatedDate,
                Roles = item.Roles.ToViewModel(),
                Permissions = item.Permissions.ToViewModel()
            });
        }

        public static ApplicationViewModel ToViewModel(this Application item)
        {
            return new ApplicationViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                CreatedDate = item.CreatedDate,
                Roles = item.Roles.ToViewModel(),
                Permissions = item.Permissions.ToViewModel()
            };
        }
    }
}
