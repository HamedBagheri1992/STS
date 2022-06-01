using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using STS.Common.Configuration;
using STS.Common.Extensions;
using STS.DataAccessLayer;
using STS.DTOs.AccountModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Services.Helper;
using STS.Services.Mappers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace STS.Services.Impls
{
    public class AccountService : IAccountService
    {
        private readonly STSDbContext _context;
        private readonly IOptionsMonitor<BearerTokensConfigurationModel> _options;

        public AccountService(STSDbContext context, IOptionsMonitor<BearerTokensConfigurationModel> options)
        {
            _context = context;
            _options = options;
        }

        public async Task<UserViewModel?> LoginAsync(LoginFormModel formModel)
        {
            try
            {
                string password = formModel.Password.ToHashPassword();

                var user = await _context.Users.Include(u => u.Applications).ThenInclude(t => t.Roles).ThenInclude(t => t.Permissions)
                    .FirstOrDefaultAsync(u => u.UserName == formModel.UserName && u.Password == password && u.Applications.Any(a => a.Id == formModel.AppId && a.SecretKey == formModel.SecretKey));

                return user?.ToViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception("AccountService : LoginError", ex);
            }
        }

        public string GenerateToken(UserViewModel user, List<PermissionViewModel> permissions)
        {
            try
            {
                return user.ToJwtTokenGenerator(_options, permissions);
            }
            catch (Exception ex)
            {
                throw new Exception("AccountService : TokenError", ex);
            }
        }

        public async Task UpdateLastLoginAsync(UserViewModel userModel)
        {
            try
            {
                var user = await _context.Users.FindAsync(userModel.Id);
                if (user is null)
                    throw new Exception("Problem on Login to System");

                user.LastLogin = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("AccountService : UpdateLastLoginError", ex);
            }
        }
    }
}
