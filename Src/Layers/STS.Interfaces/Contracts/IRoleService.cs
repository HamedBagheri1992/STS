using STS.DTOs.ResultModels;
using STS.DTOs.RoleModels.FormModels;
using STS.DTOs.RoleModels.ViewModels;

namespace STS.Interfaces.Contracts
{
    public interface IRoleService
    {
        Task<long> AddAsync(AddRoleFormModel addFormModel);
        Task<List<RoleViewModel>> GetAsync(long applicationId);
        Task<PaginatedResult<RoleViewModel>> GetAsync(long applicationId, PaginationParam pagination);
        Task<RoleViewModel?> GetAsync(long applicationId, long roleId);
        Task<RoleViewModel?> GetByIdAsync(long id);
        Task UpdateAsync(UpdateRoleFormModel updateFormModel);
        Task UpdateRolePermissionAsync(UpdateRolePermissionFormModel updateRolePermissionFormModel);
        Task DeleteAsync(long id);

        Task<bool> IsExistAsync(long id);        
        Task<bool> IsRoleValidAsync(long id);
        Task<bool> IsCaptionDuplicateAsync(long applicationId, string caption);
        Task<bool> IsCaptionDuplicateAsync(long applicationId, long id, string caption);
    }
}
