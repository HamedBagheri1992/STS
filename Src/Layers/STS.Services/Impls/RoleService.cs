using Microsoft.EntityFrameworkCore;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.RoleModels.FormModels;
using STS.DTOs.RoleModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Impls
{
    public class RoleService : IRoleService
    {
        private readonly STSDbContext _context;

        public RoleService(STSDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleViewModel>> GetAsync(long applicationId)
        {
            try
            {
                var roles = _context.Roles.Where(x => x.ApplicationId == applicationId).Include(r => r.Application).Include(r => r.Permissions).AsQueryable();
                return await roles.ToViewModel().ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("RoleService : Get(applicationId)Error");
            }
        }

        public async Task<RoleViewModel?> GetAsync(long applicationId, long roleId)
        {
            try
            {
                var role = await _context.Roles.Include(r => r.Application).Include(r => r.Permissions)
                    .FirstOrDefaultAsync(x => x.ApplicationId == applicationId && x.Id == roleId);
                return role?.ToViewModel();
            }
            catch (Exception)
            {
                throw new Exception("RoleService : Get(applicationId,roleId)Error");
            }
        }

        public async Task<RoleViewModel?> GetBgIdAsync(long roleId)
        {
            try
            {
                var role = await _context.Roles.Include(r => r.Application).Include(r => r.Permissions).FirstOrDefaultAsync(x => x.Id == roleId);
                return role?.ToViewModel();
            }
            catch (Exception)
            {
                throw new Exception("RoleService : Get(roleId)Error");
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
            catch (Exception)
            {
                throw new Exception("RoleService : AddError");
            }
        }

        public async Task UpdateAsync(UpdateRoleFormModel updateFormModel)
        {
            try
            {
                var role = await _context.Roles.FindAsync(updateFormModel.Id);
                if (role is null)
                    throw new Exception("RoleService : Role is Invalid");

                if (role.Caption != updateFormModel.Caption)
                    role.Caption = updateFormModel.Caption;

                if (role.ApplicationId != updateFormModel.ApplicationId)
                    role.ApplicationId = updateFormModel.ApplicationId;

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("RoleService : UpdateError");
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role is null)
                    throw new Exception("RoleService : Role is Invalid");

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("RoleService : DeleteError");
            }
        }

        public async Task<bool> IsCaptionDuplicateAsync(string caption)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Caption == caption);
            }
            catch (Exception)
            {
                throw new Exception("RoleService : IsCaptionDuplicateError");
            }
        }

        public async Task<bool> IsCaptionDuplicateAsync(long id, string caption)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Id != id && r.Caption == caption);
            }
            catch (Exception)
            {
                throw new Exception("RoleService : IsCaptionDuplicate(id, caption)Error");
            }
        }

        public async Task<bool> IsExistAsync(long id)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("RoleService : IsExistError");
            }
        }

        public async Task<bool> IsRoleValidAsync(long id)
        {
            try
            {
                return await _context.Roles.AnyAsync(r => r.Id == id);
            }
            catch (Exception)
            {
                throw new Exception("RoleService : IsRoleValidError");
            }
        }
    }
}
