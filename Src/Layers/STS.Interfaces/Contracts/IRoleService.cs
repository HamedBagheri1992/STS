using STS.DTOs.RoleModels.FormModels;
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
        Task<long> AddAsync(AddRoleFormModel addFormModel);
        Task<List<RoleViewModel>> GetAsync(long applicationId);
        Task<RoleViewModel?> GetAsync(long applicationId, long roleId);
        Task UpdateAsync(UpdateRoleFormModel updateFormModel);
        Task DeleteAsync(long id);

        Task<bool> IsExistAsync(long id);
        Task<bool> IsCaptionDuplicateAsync(string caption);
        Task<bool> IsRoleValidAsync(long id);
        Task<bool> IsCaptionDuplicateAsync(long id, string caption);
    }
}
