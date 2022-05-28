using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
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

    }
}
