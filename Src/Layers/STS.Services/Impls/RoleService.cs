using Microsoft.EntityFrameworkCore;
using STS.Common.BaseModels;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.ResultModels;
using STS.DTOs.RoleModels.FormModels;
using STS.DTOs.RoleModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Services.Helper;
using STS.Services.Mappers;


namespace STS.Services.Impls
{
    public class RoleService : IRoleService
    {
        private readonly STSDbContext _context;

        public RoleService(STSDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<RoleViewModel>> GetAsync(long applicationId, PaginationParam pagination)
        {
            try
            {
                var roles = _context.Roles.Where(x => x.ApplicationId == applicationId).Include(r => r.Application).Include(r => r.Permissions).ThenInclude(c => c.Category).ToViewModel();
                return await roles.ToPagedListAsync<RoleViewModel>(pagination.PageNumber, pagination.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : Get(applicationId)Error", ex);
            }
        }

        public async Task<List<RoleViewModel>> GetAsync(long applicationId)
        {
            try
            {
                return await _context.Roles.Where(x => x.ApplicationId == applicationId).Include(r => r.Application).Include(r => r.Permissions).ThenInclude(c => c.Category).ToViewModel().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : Get(applicationId)Error", ex);
            }
        }

        public async Task<RoleViewModel?> GetAsync(long applicationId, long roleId)
        {
            try
            {
                var role = await _context.Roles.Include(r => r.Application).Include(r => r.Permissions).ThenInclude(c => c.Category)
                    .FirstOrDefaultAsync(x => x.ApplicationId == applicationId && x.Id == roleId);
                return role?.ToViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : Get(applicationId,roleId)Error", ex);
            }
        }

        public async Task<RoleViewModel?> GetByIdAsync(long id)
        {
            try
            {
                var role = await _context.Roles.Include(r => r.Application).Include(r => r.Permissions).ThenInclude(c => c.Category).FirstOrDefaultAsync(x => x.Id == id);
                return role?.ToViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : Get(roleId)Error", ex);
            }
        }

        public async Task<long> AddAsync(AddRoleFormModel addFormModel)
        {
            try
            {
                var role = new Role
                {
                    ApplicationId = addFormModel.ApplicationId,
                    Caption = addFormModel.Caption
                };

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return role.Id;

            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : AddError", ex);
            }
        }

        public async Task UpdateAsync(UpdateRoleFormModel updateFormModel)
        {
            try
            {
                var role = await _context.Roles.FindAsync(updateFormModel.Id);
                if (role is null)
                    throw new STSException("RoleService : Role is Invalid");

                if (role.Caption != updateFormModel.Caption)
                    role.Caption = updateFormModel.Caption;

                if (role.ApplicationId != updateFormModel.ApplicationId)
                    role.ApplicationId = updateFormModel.ApplicationId;

                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : UpdateError", ex);
            }
        }

        public async Task UpdateRolePermissionAsync(UpdateRolePermissionFormModel updateRolePermissionFormModel)
        {
            try
            {
                var role = await _context.Roles.Include(r => r.Permissions).ThenInclude(c => c.Category).FirstOrDefaultAsync(r => r.Id == updateRolePermissionFormModel.RoleId);
                if (role is null)
                    throw new STSException("RoleService : Role is Invalid");

                var permissions = await _context.Permissions.Where(p => updateRolePermissionFormModel.PermissionIds.Contains(p.Id)).ToListAsync();
                if (permissions.Count != updateRolePermissionFormModel.PermissionIds.Count)
                    throw new STSException("Some PermissionIds are Invalid.");


                if (permissions.GroupBy(p => p.ApplicationId).Count() > 1)
                    throw new STSException("permissions are not in a same Application.");

                role.GetConsistency(updateRolePermissionFormModel, out List<Permission> deletePermissions, out List<long> addPermissionId);

                var addPermission = await _context.Permissions.Where(p => addPermissionId.Contains(p.Id)).ToListAsync();
                if (addPermissionId.Count != addPermission.Count)
                    throw new STSException("Some PermissionIds are Invalid.");

                role.Permissions.AddRange(addPermission);
                role.Permissions.RemoveRange(deletePermissions);

                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : UpdateRolePermissionError", ex);
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role is null)
                    throw new STSException("RoleService : Role is Invalid");

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : DeleteError", ex);
            }
        }

        public async Task<bool> IsCaptionDuplicateAsync(long applicationId, string caption)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Caption == caption && r.ApplicationId == applicationId);
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : IsCaptionDuplicateError", ex);
            }
        }

        public async Task<bool> IsCaptionDuplicateAsync(long applicationId, long id, string caption)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Id != id && r.Caption == caption && r.ApplicationId == applicationId);
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : IsCaptionDuplicate(id, caption)Error", ex);
            }
        }

        public async Task<bool> IsExistAsync(long id)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : IsExistError", ex);
            }
        }

        public async Task<bool> IsRoleValidAsync(long id)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("RoleService : IsRoleValidError", ex);
            }
        }

    }
}
