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
        private readonly IRoleService _roleService;

        public AccountController(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<LoginViewModel>> Login([FromBody] LoginFormModel formModel)
        {
            UserViewModel user = await _accountService.LoginAsync(formModel);

            if (user is null)
                return BadError("UserName or Password is Invalid");

            var permissions = (await _roleService.GetAsync(formModel.AppId)).SelectMany(r => r.Permissions).Distinct().ToList();

            string token = _accountService.GenerateToken(user, permissions);

            await _accountService.UpdateLastLoginAsync(user);

            return Ok(new LoginViewModel { AccessToken = token, RefreshToken = string.Empty });
        }
    }
}
