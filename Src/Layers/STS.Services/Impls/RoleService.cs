using Microsoft.EntityFrameworkCore;
using STS.DataAccessLayer;
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

        public Task<long> AddAsync(AddRoleFormModel addFormModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RoleViewModel>> GetAsync(long applicationId)
        {
            try
            {
                var roles = _context.Roles.Where(x => x.ApplicationId == applicationId).Include(r => r.Permissions).AsQueryable();
                return await roles.ToViewModel().ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("RoleService : GetError");
            }
        }

        public async Task<RoleViewModel?> GetBgIdAsync(long roleId)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                return role?.ToViewModel();
            }
            catch (Exception)
            {
                throw new Exception("RoleService : Get(id)Error");
            }
        }

        public Task<RoleViewModel?> GetAsync(long applicationId, long roleId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCaptionDuplicateAsync(string caption)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCaptionDuplicateAsync(long id, string caption)
        {
            throw new NotImplementedException();
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

        public Task<bool> IsRoleValidAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateRoleFormModel updateFormModel)
        {
            throw new NotImplementedException();
        }
    }
}
