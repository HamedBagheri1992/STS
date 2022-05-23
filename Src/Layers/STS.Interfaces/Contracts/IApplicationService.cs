using STS.DTOs.ApplicationModels.FormModels;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.CommonModels;

namespace STS.Interfaces.Contracts
{
    public interface IApplicationService
    {
        Task<PagedList<ApplicationViewModel>> GetAsync(PaginationParam pagination);
        Task<ApplicationViewModel?> GetAsync(long id);
        Task<long> AddAsync(AddApplicationFormModel addFormModel);
        Task UpdateAsync(UpdateApplicationFormModel updateFormModel);
        Task DeleteAsync(long id);

        Task<bool> IsTitleDuplicateAsync(string title);
        Task<bool> IsExistAsync(long id);
        Task<bool> IsTitleDuplicateAsync(long id, string title);
    }
}
