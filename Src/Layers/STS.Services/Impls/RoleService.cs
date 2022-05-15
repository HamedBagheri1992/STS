using STS.DTOs.RoleModels.ViewModels;
using STS.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Impls
{
    public class RoleService : IRoleService
    {
        public Task<IEnumerable<RoleViewModel>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
