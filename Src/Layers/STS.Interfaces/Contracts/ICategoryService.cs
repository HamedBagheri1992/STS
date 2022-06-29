using STS.DTOs.BaseModels;
using STS.DTOs.CategoryModels.FormModels;
using STS.DTOs.CategoryModels.ViewModels;
using STS.DTOs.ResultModels;

namespace STS.Interfaces.Contracts
{
    public interface ICategoryService
    {
        Task<CategoryViewModel?> GetAsync(long id);
        Task<PaginatedResult<CategoryViewModel>> GetAsync(PaginationParam pagination);
        Task<long> AddAsync(AddCategoryFormModel addFormModel);
        Task UpdateAsync(UpdateCategoryFormModel updateFormModel);
        Task<bool> IsExistAsync(long id);
        Task<bool> IsTitleDuplicateAsync(string title, long applicationId);
        Task<bool> IsTitleDuplicateAsync(long id, string title, long applicationId);
        Task<IEnumerable<SelectItemListModel>> GetItemListAsync(long applicationId);
    }
}
