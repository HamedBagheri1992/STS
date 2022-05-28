using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STS.Common.BaseModels;

namespace STS.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public BadRequestObjectResult BadError(string message)
        {
            var errorModel = new ErrorDetails
            {
                Status = 400,
                StatusText = "Action Error",
                Message = message
            };
            return BadRequest(errorModel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public NotFoundObjectResult NotError(string message)
        {
            var errorModel = new ErrorDetails
            {
                Status = 404,
                StatusText = "Action Error",
                Message = message
            };
            return NotFound(errorModel);
        }
    }
}
