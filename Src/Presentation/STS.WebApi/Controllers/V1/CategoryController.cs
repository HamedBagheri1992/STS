using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STS.DTOs.BaseModels;
using STS.DTOs.CategoryModels.FormModels;
using STS.DTOs.CategoryModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Interfaces.Contracts;

namespace STS.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "STS-Permission")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IApplicationService _applicationService;

        public CategoryController(ICategoryService categoryService, IApplicationService applicationService)
        {
            _categoryService = categoryService;
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<CategoryViewModel>>> Get([FromQuery] PaginationParam pagination)
        {
            var categories = await _categoryService.GetAsync(pagination);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryViewModel>> Get([FromRoute] long id)
        {
            var category = await _categoryService.GetAsync(id);
            if (category is null)
                return NotError("Category is Invalid");

            return Ok(category);
        }

        [HttpGet("[action]/{applicationId}")]
        public async Task<ActionResult<IEnumerable<SelectItemListModel>>> GetItemList([FromRoute] long applicationId)
        {
            if (await _applicationService.IsExistAsync(applicationId) == false)
                return BadError("ApplicationId is Invalid.");

            var items = await _categoryService.GetItemListAsync(applicationId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddCategoryFormModel addFormModel)
        {

            if (!await _applicationService.IsExistAsync(addFormModel.ApplicationId))
                return BadError("Application is Invalid");

            if (await _categoryService.IsTitleDuplicateAsync(addFormModel.Title, addFormModel.ApplicationId))
                return BadError("Title is Duplicate");

            long categoryId = await _categoryService.AddAsync(addFormModel);

            var addedCategory = await _categoryService.GetAsync(categoryId);
            if (addedCategory is null)
                return NotError("Category Added Problem");

            return CreatedAtAction(nameof(Get), new { userId = addedCategory.Id }, addedCategory);
        }


        [HttpPut]
        public async Task<IActionResult> Put(UpdateCategoryFormModel updateFormModel)
        {

            if (!await _applicationService.IsExistAsync(updateFormModel.ApplicationId))
                return BadError("Application is Invalid");

            if (!await _categoryService.IsExistAsync(updateFormModel.Id))
                return BadError("Category is Invalid");

            if (await _categoryService.IsTitleDuplicateAsync(updateFormModel.Id, updateFormModel.Title, updateFormModel.ApplicationId))
                return BadError("Title is Duplicate");

            await _categoryService.UpdateAsync(updateFormModel);
            return NoContent();
        }

    }
}
