using Microsoft.AspNetCore.Mvc;
using STS.Common.BaseModels;

namespace STS.WebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ChangeModelStateInvalidModel(this ApiBehaviorOptions opt)
        {
            opt.InvalidModelStateResponseFactory = (context => new BadRequestObjectResult(new ErrorDetails
            {
                ErrorCode = $"{context.RouteData.Values["controller"]}-{context.RouteData.Values["action"]}-ModelError",
                Messages = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()
            }));
        }
    }
}
