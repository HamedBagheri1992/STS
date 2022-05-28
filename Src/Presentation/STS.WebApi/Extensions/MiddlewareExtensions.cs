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
                Status = 400,
                StatusText = $"{context.RouteData.Values["controller"]}-{context.RouteData.Values["action"]}-ModelError",
                Error = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()
            }));
        }
    }
}
