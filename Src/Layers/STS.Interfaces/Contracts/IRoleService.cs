using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Interfaces.Contracts
{
    public interface IRoleService
    {
        Task<bool> IsExistAsync(int id);
        Task<IEnumerable<RoleViewModel>> GetAsync();
    }
}
