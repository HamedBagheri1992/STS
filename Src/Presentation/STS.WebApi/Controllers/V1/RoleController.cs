using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STS.DTOs.RoleModels.FormModels;
using STS.DTOs.RoleModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.WebApi.Helper;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        public async Task<ActionResult<IEnumerable<RoleViewModel>>> Get(long applicationId)
        {
            var roles = await _roleService.GetAsync(applicationId);
            return Ok(roles);
        }

        [HttpGet("{applicationId}/{roleId}")]
        public async Task<ActionResult<RoleViewModel>> Get(long applicationId, long roleId)
        {
            var role = await _roleService.GetAsync(applicationId, roleId);
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddRoleFormModel addFormModel)
        {
            if (!await _applicationService.IsExistAsync(addFormModel.ApplicationId))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("ApplicationId is Invalid"));

            if (await _roleService.IsCaptionDuplicateAsync(addFormModel.Caption))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Role Caption is Duplicate"));

            long roleId = await _roleService.AddAsync(addFormModel);

            var addedRole = await _roleService.GetAsync(addFormModel.ApplicationId, roleId);
            if (addedRole is null)
                return NotFound(ErrorDetailsHelper.VerificationErrorDetails("Role Added Problem"));

            return CreatedAtAction(nameof(Get), new { applicationId = addedRole.ApplicationId, roleId = addedRole.Id }, addedRole);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateRoleFormModel updateFormModel)
        {
            if (!await _applicationService.IsExistAsync(updateFormModel.ApplicationId))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Application is Invalid"));

            if (!await _roleService.IsRoleValidAsync(updateFormModel.Id))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Role is Invalid"));

            if (await _roleService.IsCaptionDuplicateAsync(updateFormModel.Id, updateFormModel.Caption))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Role Caption is Duplicate"));


            await _roleService.UpdateAsync(updateFormModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!await _roleService.IsRoleValidAsync(id))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Role is Invalid"));

            await _roleService.DeleteAsync(id);
            return NoContent();
        }
    }
}
