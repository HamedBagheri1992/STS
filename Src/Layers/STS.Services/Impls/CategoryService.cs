using Microsoft.EntityFrameworkCore;
using STS.Common.BaseModels;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.BaseModels;
using STS.DTOs.CategoryModels.FormModels;
using STS.DTOs.CategoryModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Interfaces.Contracts;
using STS.Services.Mappers;

namespace STS.Services.Impls
{
    public class CategoryService : ICategoryService
    {
        private readonly STSDbContext _context;

        public CategoryService(STSDbContext context)
        {
            _context = context;
        }

        public async Task<long> AddAsync(AddCategoryFormModel addFormModel)
        {
            try
            {
                var category = new Category
                {
                    ApplicationId = addFormModel.ApplicationId,
                    Title = addFormModel.Title
                };

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return category.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("CategoryService : AddError", ex);
            }
        }

        public async Task<CategoryViewModel?> GetAsync(long id)
        {
            try
            {
                var category = await _context.Categories.Include(c => c.Application).FirstOrDefaultAsync(c => c.Id == id);
                return category?.ToViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception("CategoryService : Get(Id)Error", ex);
            }
        }

        public async Task<PaginatedResult<CategoryViewModel>> GetAsync(PaginationParam pagination)
        {
            try
            {
                var users = _context.Categories.Include(c => c.Application).OrderBy(c => c.ApplicationId).ToViewModel();
                return await users.ToPagedListAsync<CategoryViewModel>(pagination.PageNumber, pagination.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("CategoryService : GetError", ex);
            }
        }

        public async Task UpdateAsync(UpdateCategoryFormModel updateFormModel)
        {
            try
            {
                var category = await _context.Categories.FindAsync(updateFormModel.Id);
                if (category is null)
                    throw new STSException("Category is Invalid");

                if (category.ApplicationId != updateFormModel.ApplicationId)
                    category.ApplicationId = updateFormModel.ApplicationId;

                if (category.Title != updateFormModel.Title)
                    category.Title = updateFormModel.Title;


                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("CategoryService : UpdateError", ex);
            }
        }

        public async Task<bool> IsExistAsync(long id)
        {
            try
            {
                return await _context.Categories.AnyAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("CategoryService : IsExistError", ex);
            }
        }

        public async Task<bool> IsTitleDuplicateAsync(string title, long applicationId)
        {
            try
            {
                return await _context.Categories.AnyAsync(c => c.ApplicationId == applicationId && c.Title == title);
            }
            catch (Exception ex)
            {
                throw new Exception("CategoryService : IsTitleDuplicate(Title)Error", ex);
            }
        }

        public async Task<bool> IsTitleDuplicateAsync(long id, string title, long applicationId)
        {
            try
            {
                return await _context.Categories.AnyAsync(c => c.ApplicationId == applicationId && c.Title == title && c.Id != id);
            }
            catch (Exception ex)
            {
                throw new Exception("CategoryService : IsTitleDuplicate(Id,Title)Error", ex);
            }
        }

        public async Task<IEnumerable<SelectItemListModel>> GetItemListAsync(long applicationId)
        {
            try
            {
                return await _context.Categories.Where(c => c.ApplicationId == applicationId).ToSelectItemList().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("CategoryService : GetItemListError", ex);
            }
        }
    }
}
