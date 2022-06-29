using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STS.DTOs.OrganizationModel.FormModels;
using STS.DTOs.OrganizationModel.ViewModels;
using STS.Interfaces.Contracts;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "STS-Permission")]
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationService _organizationService;
        public readonly IUserService _userService;

        public OrganizationController(IOrganizationService organizationService, IUserService userService)
        {
            _organizationService = organizationService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationViewModel>>> Get()
        {
            var organizations = await _organizationService.GetAsync();
            return Ok(organizations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrganizationViewModel>>> Get([FromRoute] long id)
        {
            var organizations = await _organizationService.GetAsync(id);
            return Ok(organizations);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<OrganizationViewModel>> GetSingleOrganization([FromRoute] long id)
        {
            OrganizationViewModel organization = await _organizationService.GetSingleOrganizationAsync(id);
            if (organization is null)
                return NotError("Organization is Invalid");

            return Ok(organization);
        }

        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<OrganizationViewModel>> GetUserOrganizations([FromRoute] long userId)
        {
            var user = await _userService.GetAsync(userId);
            if (user is null)
                return BadError("User is Invalid");

            if (!user.OrganizationIds.Any())
                return NotError("There is not any Organization for this User.");

            var organizations = await _organizationService.GetAsync(user);
            return Ok(organizations);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddOrganizationFormModel addFormModel)
        {
            if (addFormModel.ParentId.HasValue)
            {
                if (!await _organizationService.IsExistAsync(addFormModel.ParentId.Value))
                    return BadError("ParentId is Invalid");
            }

            var addedId = await _organizationService.AddAsync(addFormModel);

            var addedOrg = await _organizationService.GetSingleOrganizationAsync(addedId);

            if (addedOrg is null)
                return BadError("Organization Added had problem.");

            return CreatedAtAction(nameof(GetSingleOrganization), new { id = addedId }, addedOrg);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateOrganizationFormModel updateFormModel)
        {
            if (!await _organizationService.IsExistAsync(updateFormModel.Id))
                return BadError("Organization is Invalid");

            await _organizationService.UpdateAsync(updateFormModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!await _organizationService.IsExistAsync(id))
                return BadError("Organization is Invalid");

            await _organizationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
