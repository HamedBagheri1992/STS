using Microsoft.AspNetCore.Mvc;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.Interfaces.Contracts;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;

        public PermissionController(IPermissionService permissionService, IRoleService roleService)
        {
            _permissionService = permissionService;
            _roleService = roleService;
        }

        [HttpGet("{roleId}")]
        public async Task<ActionResult<IEnumerable<PermissionViewModel>>> Get(long roleId)
        {
            var permissions = await _permissionService.GetAsync(roleId);
            return Ok(permissions);
        }

        [HttpGet("{roleId}/{permissionId}")]
        public async Task<ActionResult<PermissionViewModel>> Get(long roleId, long permissionId)
        {
            var permission = await _permissionService.GetAsync(roleId, permissionId);
            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddPermissionFormModel addFormModel)
        {
            if (!await _roleService.IsExistAsync(addFormModel.RoleId))
                return BadError("RoleId is Invalid");

            if (await _permissionService.IsTitleDuplicateAsync(addFormModel.Title))
                return BadError("Permission Title is Duplicate");

            long permissionId = await _permissionService.AddAsync(addFormModel);

            var addedPermission = await _permissionService.GetAsync(addFormModel.RoleId, permissionId);
            if (addedPermission is null)
                return NotError("Permission Added Problem");

            return CreatedAtAction(nameof(Get), new { roleId = addedPermission.RoleId, permissionId = addedPermission.Id }, addedPermission);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdatePermissionFormModel updateFormModel)
        {
            if (!await _roleService.IsExistAsync(updateFormModel.RoleId))
                return BadError("RoleId is Invalid");

            if (!await _permissionService.IsPermissionValidAsync(updateFormModel.Id))
                return BadError("Permission is Invalid");

            if (await _permissionService.IsTitleDuplicateAsync(updateFormModel.Id, updateFormModel.Title))
                return BadError("Permission Title is Duplicate");


            await _permissionService.UpdateAsync(updateFormModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!await _permissionService.IsPermissionValidAsync(id))
                return BadError("Permission is Invalid");

            await _permissionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
