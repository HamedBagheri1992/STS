using Microsoft.EntityFrameworkCore;
using STS.DataAccessLayer;
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

        public async Task<List<RoleViewModel>> GetAsync()
        {
            try
            {
                var roles = _context.Roles.Include(r => r.Permissions).AsQueryable();
                return await roles.ToViewModel().ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("RoleService : GetError");
            }
        }

        public async Task<RoleViewModel?> GetAsync(long roleId)
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
    }
}
