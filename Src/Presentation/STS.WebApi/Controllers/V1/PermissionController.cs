using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STS.DTOs.PermissionModels.FormModels;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Interfaces.Contracts;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "STS-Permission")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;
        private readonly IApplicationService _applicationService;

        public PermissionController(IPermissionService permissionService, IApplicationService applicationService)
        {
            _permissionService = permissionService;
            _applicationService = applicationService;
        }


        [HttpGet("{applicationId}")]
        public async Task<ActionResult<PaginatedResult<PermissionViewModel>>> Get([FromRoute] long applicationId, [FromQuery] PaginationParam pagination)
        {
            var permissions = await _permissionService.GetAsync(applicationId, pagination);
            return Ok(permissions);
        }

        [HttpGet("{applicationId}/{permissionId}")]
        public async Task<ActionResult<PermissionViewModel>> Get([FromRoute] long applicationId, [FromRoute] long permissionId)
        {
            var permission = await _permissionService.GetAsync(applicationId, permissionId);
            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddPermissionFormModel addFormModel)
        {
            if (!await _applicationService.IsExistAsync(addFormModel.ApplicationId))
                return BadError("Application is Invalid");

            if (await _permissionService.IsTitleDuplicateAsync(addFormModel.ApplicationId, addFormModel.Title))
                return BadError("Permission Title is Duplicate");

            long permissionId = await _permissionService.AddAsync(addFormModel);

            var addedPermission = await _permissionService.GetAsync(addFormModel.ApplicationId, permissionId);
            if (addedPermission is null)
                return NotError("Permission Added Problem");
            return CreatedAtAction(nameof(Get), new { applicationId = addedPermission.ApplicationId, permissionId = addedPermission.Id }, addedPermission);
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePermissionCategory([FromBody] UpdatePermissionCategoryFormModel addFormModel)
        {
            if (!addFormModel.PermissionIds.Any())
                return BadError("There is not any Permission");

            await _permissionService.UpdatePermissionCategoryAsync(addFormModel);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdatePermissionFormModel updateFormModel)
        {
            if (!await _applicationService.IsExistAsync(updateFormModel.ApplicationId))
                return BadError("ApplicationId is Invalid");

            if (!await _permissionService.IsPermissionValidAsync(updateFormModel.Id))
                return BadError("Permission is Invalid");

            if (await _permissionService.IsTitleDuplicateAsync(updateFormModel.ApplicationId, updateFormModel.Id, updateFormModel.Title))
                return BadError("Permission Title is Duplicate");


            await _permissionService.UpdateAsync(updateFormModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (!await _permissionService.IsPermissionValidAsync(id))
                return BadError("Permission is Invalid");

            await _permissionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
