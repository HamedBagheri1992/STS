using Microsoft.AspNetCore.Mvc;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.WebApi.Filters;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;

        public PermissionController(IPermissionService permissionService, IRoleService roleService)
        {
            _permissionService = permissionService;
            _roleService = roleService;
        }

        [HttpGet("[action]/{roleId}")]
        public async Task<ActionResult<IEnumerable<PermissionViewModel>>> Get(int roleId)
        {
            var permissions = await _permissionService.GetAsync(roleId);
            return Ok(permissions);
        }

        [HttpGet("[action]/{roleId}/{permissionId}")]
        public async Task<ActionResult<PermissionViewModel>> Get(int roleId, int permissionId)
        {
            var permission = await _permissionService.GetAsync(roleId, permissionId);
            return Ok(permission);
        }

        [HttpPost]     
        public async Task<IActionResult> Post([FromBody] AddPermissionFormModel addFormModel)
        {
            if (!await _roleService.IsExistAsync(addFormModel.RoleId))
                return NotFound("RoleId is Invalid");

            var addedPermission = await _permissionService.AddAsync(addFormModel);
            return CreatedAtAction(nameof(Get), new { roleId = addedPermission.RoleId, permissionId = addedPermission.Id }, addedPermission);
        }


        // PUT: api/values/5
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/values/5
        [HttpDelete]
        public void Delete()
        {
            throw new Exception("hamed");
        }
    }
}
