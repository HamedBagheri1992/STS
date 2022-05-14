using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Impls
{
    public class PermissionService : IPermissionService
    {
        public Task<PermissionViewModel> AddAsync(AddPermissionFormModel addFormModel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PermissionViewModel>> GetAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<PermissionViewModel> GetAsync(int roleId, int permissionId)
        {
            throw new NotImplementedException();
        }
    }
}
