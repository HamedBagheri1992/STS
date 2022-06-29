using STS.Common.BaseModels;
using STS.DTOs.AccountModels.FormModels;

namespace STS.Interfaces.Contracts
{
    public interface IAccountService
    {
        Task<UserIdentityBaseModel?> LoginAsync(LoginFormModel formModel);
        string GenerateToken(UserIdentityBaseModel user);
        Task UpdateLastLoginAsync(UserIdentityBaseModel user);
    }
}
