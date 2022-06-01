using Microsoft.EntityFrameworkCore;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
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
                    DisplayTitle = addFormModel.DisplayTitle,
                    Title = addFormModel.Title
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

        public async Task<List<PermissionViewModel>> GetAsync(long applicationId)
        {
            try
            {
                return await _context.Permissions.Include(p => p.Roles).Where(r => r.ApplicationId == applicationId).ToViewModel().ToListAsync();
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
                var permission = await _context.Permissions.Include(p => p.Roles).FirstOrDefaultAsync(p => p.Id == permissionId && p.ApplicationId == applicationId);
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
                    throw new Exception("PermissionService : Permission is Invalid");

                if (permission.Title != updateFormModel.Title)
                    permission.Title = updateFormModel.Title;

                if (permission.DisplayTitle != updateFormModel.DisplayTitle)
                    permission.DisplayTitle = updateFormModel.DisplayTitle;

                if (permission.ApplicationId != updateFormModel.ApplicationId)
                    permission.ApplicationId = updateFormModel.ApplicationId;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : UpdateError", ex);
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var permission = await _context.Permissions.FindAsync(id);
                if (permission is null)
                    throw new Exception("PermissionService : Permission is Invalid");

                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();
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

        public async Task<bool> IsTitleDuplicateAsync(string title)
        {
            try
            {
                return await _context.Permissions.AnyAsync(p => p.Title == title);
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : IsTitleDuplicate(title)Error", ex);
            }
        }

        public async Task<bool> IsTitleDuplicateAsync(long permissionId, string title)
        {
            try
            {
                return await _context.Permissions.AnyAsync(p => p.Title == title && p.Id != permissionId);
            }
            catch (Exception ex)
            {
                throw new Exception("PermissionService : IsTitleDuplicate(permissionId,title)Error", ex);
            }
        }
    }
}
