using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using STS.Common.BaseModels;

namespace STS.WebApi.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Select(m => m.Errors);
                context.Result = new BadRequestObjectResult(new ErrorDetails
                {
                    ErrorCode = "100",
                    Messages = errors.Select(e => e.ToString()).ToList()
                }.ToString());
            }
        }
    }
}
