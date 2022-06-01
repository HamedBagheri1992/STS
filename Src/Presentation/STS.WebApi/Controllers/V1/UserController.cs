using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STS.Common.Extensions;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.FormModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "STS-Permission")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<ActionResult<PaginatedResult<UserViewModel>>> Get([FromQuery] PaginationParam pagination)
        {
            var users = await _userService.GetAsync(pagination);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> Get([FromRoute] long id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddUserFormModel addFormModel)
        {
            if (string.IsNullOrEmpty(addFormModel.Password))
                return BadError("Password is required");

            if (addFormModel.UserName.HasSpecialChars())
                return BadError("UserName must be Digit and Letter");

            if (await _userService.IsUserNameDuplicateAsync(addFormModel.UserName))
                return BadError("UserName is Duplicate");

            long userId = await _userService.AddAsync(addFormModel);

            var addedUser = await _userService.GetAsync(userId);
            if (addedUser is null)
                return NotError("User Added Problem");

            return CreatedAtAction(nameof(Get), new { userId = addedUser.Id }, addedUser);
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUserFormModel updateFormModel)
        {
            if (updateFormModel.UserName.HasSpecialChars())
                return BadError("UserName must be Digit and Letter");

            if (!await _userService.IsExistAsync(updateFormModel.Id))
                return BadError("User is Invalid");

            if (await _userService.IsUserNameDuplicateAsync(updateFormModel.Id, updateFormModel.UserName))
                return BadError("UserName is Duplicate");


            await _userService.UpdateAsync(updateFormModel);
            return NoContent();
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordFormModel changePasswordFormModel)
        {
            if (!await _userService.IsPasswordValidAsync(changePasswordFormModel.Id, changePasswordFormModel.oldPassword))
                return BadError("Password is Invalid");

            await _userService.ChangePasswordAsync(changePasswordFormModel);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (!await _userService.IsExistAsync(id))
                return BadError("User is Invalid");

            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
