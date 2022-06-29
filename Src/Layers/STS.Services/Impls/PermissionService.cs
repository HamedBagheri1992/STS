using Microsoft.EntityFrameworkCore;
using STS.Common.BaseModels;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Interfaces.Contracts;
using STS.Services.Mappers;

namespace STS.Services.Impls
{
    public class PermissionService : IPermissionService
    {
        private readonly STSDbContext _context;

        public PermissionService(STSDbContext context)
        {
            _context = context;
        }

        public async Task<long> AddAsync(AddPermissionFormModel addFormModel)
        {
            try
            {
                var permission = new Permission
                {
                    ApplicationId = addFormModel.ApplicationId,
                    DisplayTitle = addFormModel.DisplayTitle,
                    Title = addFormModel.Title,
                    CategoryId = addFormModel.CategoryId
                };

                await _context.Permissions.AddAsync(permission);
                await _context.SaveChangesAsync();

                return permission.Id;

            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : AddError", ex);
            }
        }

        public async Task<PaginatedResult<PermissionViewModel>> GetAsync(long applicationId, PaginationParam pagination)
        {
            try
            {
                var permissions = _context.Permissions.Include(p => p.Roles).Include(p => p.Category).Where(r => r.ApplicationId == applicationId).ToViewModel();
                return await permissions.ToPagedListAsync<PermissionViewModel>(pagination.PageNumber, pagination.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : Get(roleId)Error", ex);
            }
        }

        public async Task<PermissionViewModel?> GetAsync(long applicationId, long permissionId)
        {
            try
            {
                var permission = await _context.Permissions.Include(p => p.Roles).Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == permissionId && p.ApplicationId == applicationId);
                return permission?.ToViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : Get(roleId,permissionId)Error", ex);
            }
        }

        public async Task UpdateAsync(UpdatePermissionFormModel updateFormModel)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(updateFormModel.Id);
                if (permission is null)
                    throw new STSException("PermissionService : Permission is Invalid");

                if (permission.Title != updateFormModel.Title)
                    permission.Title = updateFormModel.Title;

                if (permission.DisplayTitle != updateFormModel.DisplayTitle)
                    permission.DisplayTitle = updateFormModel.DisplayTitle;

                if (permission.ApplicationId != updateFormModel.ApplicationId)
                    permission.ApplicationId = updateFormModel.ApplicationId;

                if (permission.CategoryId != updateFormModel.CategoryId)
                    permission.CategoryId = updateFormModel.CategoryId;

                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : UpdateError", ex);
            }
        }

        public async Task UpdatePermissionCategoryAsync(UpdatePermissionCategoryFormModel updateFormModel)
        {
            try
            {
                var permissions = _context.Permissions.Where(p => updateFormModel.PermissionIds.Contains(p.Id)).ToList();
                if (permissions.Count != updateFormModel.PermissionIds.Count)
                    throw new STSException("Some of the Permissions are invalid");

                if (permissions.GroupBy(x => x.ApplicationId).Count() > 1)
                    throw new STSException("Permission are not in an Application");

                var permissionRange = permissions.Select(x => new Permission { Id = x.Id, Title = x.Title, DisplayTitle = x.DisplayTitle, ApplicationId = x.ApplicationId, CategoryId = updateFormModel.CategoryId }).ToList();

                _context.Permissions.UpdateRange(permissionRange);
                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : AddPermissionCategoryError", ex);
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(id);
                if (permission is null)
                    throw new STSException("PermissionService : Permission is Invalid");

                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : DeleteError", ex);
            }
        }

        public async Task<bool> IsPermissionValidAsync(long id)
        {
            try
            {
                return await _context.Permissions.AnyAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : IsPermissionValidError", ex);
            }
        }

        public async Task<bool> IsTitleDuplicateAsync(long applicationId, string title)
        {
            try
            {
                return await _context.Permissions.AnyAsync(p => p.Title == title && p.ApplicationId == applicationId);
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : IsTitleDuplicate(applicationId,title)Error", ex);
            }
        }

        public async Task<bool> IsTitleDuplicateAsync(long applicationId, long permissionId, string title)
        {
            try
            {
                return await _context.Permissions.AnyAsync(p => p.Title == title && p.ApplicationId == applicationId && p.Id != permissionId);
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : IsTitleDuplicate(applicationId,permissionId,title)Error", ex);
            }
        }
    }
}
