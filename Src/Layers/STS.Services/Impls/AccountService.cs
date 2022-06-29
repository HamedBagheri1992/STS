using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using STS.Common.BaseModels;
using STS.Common.Configuration;
using STS.Common.Extensions;
using STS.DataAccessLayer;
using STS.DTOs.AccountModels.FormModels;
using STS.Interfaces.Contracts;
using STS.Services.Helper;
using STS.Services.Mappers;

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

        public async Task<UserIdentityBaseModel?> LoginAsync(LoginFormModel formModel)
        {
            try
            {
                string password = formModel.Password.ToHashPassword();

                var user = await _context.Users.Include(u => u.Applications).Include(t => t.Roles).ThenInclude(t => t.Permissions)
                    .FirstOrDefaultAsync(u => u.UserName == formModel.UserName && u.Password == password && u.Applications.Any(a => a.Id == formModel.AppId && a.SecretKey == formModel.SecretKey));

                if (user is null)
                    return null;

                var application = user.Applications.First(a => a.Id == formModel.AppId);
                var roles = user.ToRoleKeyValueModel().ToList();
                var permissions = user.ToPermissionKeyValueModel().ToList();

                var userIdentity = user.ToUserIdentityBaseModel(application, roles, permissions);
                return userIdentity;
            }
            catch (Exception ex)
            {
                throw new Exception("AccountService : LoginError", ex);
            }
        }

        public string GenerateToken(UserIdentityBaseModel userIdentity)
        {
            try
            {
                return userIdentity.ToJwtTokenGenerator(_options);
            }
            catch (Exception ex)
            {
                throw new Exception("AccountService : TokenError", ex);
            }
        }

        public async Task UpdateLastLoginAsync(UserIdentityBaseModel userModel)
        {
            try
            {
                var user = await _context.Users.FindAsync(userModel.Id);
                if (user is null)
                    throw new STSException("Problem on Login to System");

                user.LastLogin = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("AccountService : UpdateLastLoginError", ex);
            }
        }
    }
}
