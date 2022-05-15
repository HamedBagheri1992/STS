using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;


namespace STS.Interfaces.Contracts
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionViewModel>> GetAsync(int roleId);
        Task<PermissionViewModel> GetAsync(int roleId, int permissionId);
        Task<PermissionViewModel> AddAsync(AddPermissionFormModel addFormModel);
        Task UpdateAsync(UpdatePermissionFormModel updateFormModel);
        Task<bool> IsPermissionValidAsync(long id);
        Task<bool> IsTitleDuplicateAsync(string title);
        Task DeleteAsync(long id);
    }
}
