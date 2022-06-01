using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;


namespace STS.Interfaces.Contracts
{
    public interface IPermissionService
    {
        Task<List<PermissionViewModel>> GetAsync(long applicationId);
        Task<PermissionViewModel?> GetAsync(long applicationId, long permissionId);
        Task<long> AddAsync(AddPermissionFormModel addFormModel);
        Task UpdateAsync(UpdatePermissionFormModel updateFormModel);
        Task DeleteAsync(long id);

        Task<bool> IsPermissionValidAsync(long id);
        Task<bool> IsTitleDuplicateAsync(string title);
        Task<bool> IsTitleDuplicateAsync(long permissionId, string title);
    }
}
