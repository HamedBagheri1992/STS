using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STS.DTOs.ResultModels;
using STS.DTOs.RoleModels.FormModels;
using STS.DTOs.RoleModels.ViewModels;
using STS.Interfaces.Contracts;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "STS-Permission")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IApplicationService _applicationService;

        public RoleController(IApplicationService applicationService, IRoleService roleService)
        {
            _applicationService = applicationService;
            _roleService = roleService;
        }

        [HttpGet("{applicationId}")]
        public async Task<ActionResult<PaginatedResult<RoleViewModel>>> Get([FromRoute] long applicationId, [FromQuery] PaginationParam pagination)
        {
            var roles = await _roleService.GetAsync(applicationId, pagination);
            return Ok(roles);
        }

        [HttpGet("{applicationId}/{roleId}")]
        public async Task<ActionResult<RoleViewModel>> Get([FromRoute] long applicationId, [FromRoute] long roleId)
        {
            var role = await _roleService.GetAsync(applicationId, roleId);
            if (role is null)
                return NotError("RoleId is Invalid");

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddRoleFormModel addFormModel)
        {
            if (!await _applicationService.IsExistAsync(addFormModel.ApplicationId))
                return BadError("ApplicationId is Invalid");

            if (await _roleService.IsCaptionDuplicateAsync(addFormModel.ApplicationId, addFormModel.Caption))
                return BadError("Role Caption is Duplicate");

            long roleId = await _roleService.AddAsync(addFormModel);

            var addedRole = await _roleService.GetAsync(addFormModel.ApplicationId, roleId);
            if (addedRole is null)
                return NotError("Role Added Problem");

            return CreatedAtAction(nameof(Get), new { applicationId = addedRole.ApplicationId, roleId = addedRole.Id }, addedRole);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateRolePermission([FromBody] UpdateRolePermissionFormModel updateRolePermissionFormModel)
        {
            await _roleService.UpdateRolePermissionAsync(updateRolePermissionFormModel);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateRoleFormModel updateFormModel)
        {
            if (!await _applicationService.IsExistAsync(updateFormModel.ApplicationId))
                return BadError("Application is Invalid");

            if (!await _roleService.IsRoleValidAsync(updateFormModel.Id))
                return BadError("Role is Invalid");

            if (await _roleService.IsCaptionDuplicateAsync(updateFormModel.ApplicationId, updateFormModel.Id, updateFormModel.Caption))
                return BadError("Role Caption is Duplicate");


            await _roleService.UpdateAsync(updateFormModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!await _roleService.IsRoleValidAsync(id))
                return BadError("Role is Invalid");

            await _roleService.DeleteAsync(id);
            return NoContent();
        }
    }
}
