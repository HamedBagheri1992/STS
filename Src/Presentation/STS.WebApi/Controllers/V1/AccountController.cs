using Microsoft.AspNetCore.Mvc;
using STS.DTOs.AccountModels.FormModels;
using STS.DTOs.AccountModels.ViewModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
       
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<LoginViewModel>> Login([FromBody] LoginFormModel formModel)
        {
            var userIdentity = await _accountService.LoginAsync(formModel);

            if (userIdentity is null)
                return NotError("UserName or Password is Invalid");

            if (!userIdentity.IsActive)
                return BadError("User is Deactive.");

            if (userIdentity.ExpirationDate.HasValue && userIdentity.ExpirationDate.Value < DateTime.Now)
                return BadError("User has expired, Please call the administrator.");

            string token = _accountService.GenerateToken(userIdentity);

            await _accountService.UpdateLastLoginAsync(userIdentity);

            return Ok(new LoginViewModel { AccessToken = token, RefreshToken = string.Empty });
        }
    }
}
