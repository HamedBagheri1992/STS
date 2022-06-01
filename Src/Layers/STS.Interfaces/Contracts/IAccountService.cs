using Microsoft.AspNetCore.Authentication.JwtBearer;
using STS.DTOs.AccountModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.UserModels.ViewModels;

namespace STS.Interfaces.Contracts
{
    public interface IAccountService
    {
        Task<UserViewModel?> LoginAsync(LoginFormModel formModel);
        string GenerateToken(UserViewModel user, List<PermissionViewModel> permissions);
        Task UpdateLastLoginAsync(UserViewModel user);
    }
}
