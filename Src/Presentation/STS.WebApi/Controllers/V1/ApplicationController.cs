using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STS.DTOs.ApplicationModels.FormModels;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.CommonModels;
using STS.Interfaces.Contracts;
using STS.WebApi.Helper;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApplicationController : BaseController
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ApplicationViewModel>>> Get([FromQuery] PaginationParam pagination)
        {
            var applications = await _applicationService.GetAsync(pagination);
            return Ok(applications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationViewModel>> Get([FromRoute] long id)
        {
            var application = await _applicationService.GetAsync(id);
            return Ok(application);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddApplicationFormModel addFormModel)
        {
            if (await _applicationService.IsTitleDuplicateAsync(addFormModel.Title))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Application Title is Duplicate"));

            long applicationId = await _applicationService.AddAsync(addFormModel);

            var addedApp = await _applicationService.GetAsync(applicationId);
            if (addedApp is null)
                return NotFound(ErrorDetailsHelper.VerificationErrorDetails("Application Added Problem"));

            return CreatedAtAction(nameof(Get), new { applicationId = addedApp.Id }, addedApp);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateApplicationFormModel updateFormModel)
        {
            if (!await _applicationService.IsExistAsync(updateFormModel.Id))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Application is Invalid"));

            if (await _applicationService.IsTitleDuplicateAsync(updateFormModel.Id, updateFormModel.Title))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Application Title is Duplicate"));


            await _applicationService.UpdateAsync(updateFormModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!await _applicationService.IsExistAsync(id))
                return BadRequest(ErrorDetailsHelper.VerificationErrorDetails("Application is Invalid"));

            await _applicationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
