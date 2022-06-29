using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.ResultModels;

namespace STS.Interfaces.Contracts
{
    public interface IPermissionService
    {
        Task<PaginatedResult<PermissionViewModel>> GetAsync(long applicationId, PaginationParam pagination);
        Task<PermissionViewModel?> GetAsync(long applicationId, long permissionId);
        Task<long> AddAsync(AddPermissionFormModel addFormModel);
        Task UpdateAsync(UpdatePermissionFormModel updateFormModel);
        Task UpdatePermissionCategoryAsync(UpdatePermissionCategoryFormModel addFormModel);
        Task DeleteAsync(long id);

        Task<bool> IsPermissionValidAsync(long id);
        Task<bool> IsTitleDuplicateAsync(long applicationId, string title);
        Task<bool> IsTitleDuplicateAsync(long applicationId, long permissionId, string title);
    }
}
