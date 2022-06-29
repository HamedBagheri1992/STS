using STS.DTOs.OrganizationModel.FormModels;
using STS.DTOs.OrganizationModel.ViewModels;
using STS.DTOs.UserModels.ViewModels;

namespace STS.Interfaces.Contracts
{
    public interface IOrganizationService
    {
        Task<List<OrganizationViewModel>> GetAsync();
        Task<List<OrganizationViewModel>> GetAsync(long id);
        Task<List<OrganizationViewModel>> GetAsync(UserViewModel user);
        Task<OrganizationViewModel?> GetSingleOrganizationAsync(long id);
        Task<long> AddAsync(AddOrganizationFormModel addFormModel);
        Task UpdateAsync(UpdateOrganizationFormModel updateFormModel);
        Task DeleteAsync(long id);

        Task<bool> IsExistAsync(long id);       
    }
}
