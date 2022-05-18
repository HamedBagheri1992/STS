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
                    RoleId = addFormModel.RoleId,
                    Title = addFormModel.Title
                };

                await _context.Permissions.AddAsync(permission);
                await _context.SaveChangesAsync();

                return permission.Id;

            }
            catch (Exception)
            {
                throw new Exception("PermissionService : AddError");
            }
        }

        public async Task<List<PermissionViewModel>> GetAsync(long roleId)
        {
            try
            {
                return await _context.Permissions.Include(p => p.Role).Where(p => p.RoleId == roleId).ToViewModel().ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("PermissionService : Get(roleId)Error");
            }
        }

        public async Task<PermissionViewModel?> GetAsync(long roleId, long permissionId)
        {
            try
            {
                var permission = await _context.Permissions.Include(p => p.Role).FirstOrDefaultAsync(p => p.Id == permissionId && p.RoleId == roleId);
                return permission?.ToViewModel();
            }
            catch (Exception)
            {
                throw new Exception("PermissionService : Get(roleId,permissionId)Error");
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

                if (permission.RoleId != updateFormModel.RoleId)
                    permission.RoleId = updateFormModel.RoleId;

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("PermissionService : UpdateError");
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
            catch (Exception)
            {
                throw new Exception("PermissionService : DeleteError");
            }
        }

        public async Task<bool> IsPermissionValidAsync(long id)
        {
            try
            {
                return await _context.Permissions.AnyAsync(p => p.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("PermissionService : IsPermissionValidError");
            }
        }

        public async Task<bool> IsTitleDuplicateAsync(string title)
        {
            try
            {
                return await _context.Permissions.AnyAsync(p => p.Title == title);
            }
            catch (Exception)
            {
                throw new Exception("PermissionService : IsTitleDuplicate(title)Error");
            }
        }

        public async Task<bool> IsTitleDuplicateAsync(long permissionId, string title)
        {
            try
            {
                return await _context.Permissions.AnyAsync(p => p.Title == title && p.Id != permissionId);
            }
            catch (Exception)
            {
                throw new Exception("PermissionService : IsTitleDuplicate(permissionId,title)Error");
            }
        }

    }
}
