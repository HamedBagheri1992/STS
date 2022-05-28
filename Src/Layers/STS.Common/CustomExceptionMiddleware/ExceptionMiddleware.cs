using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using STS.Common.BaseModels;
using System.Net;

namespace STS.Common.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        public readonly RequestDelegate _next;
        public readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} : {ex}");
                await HandleExceptionAsync(httpContext, ex.Message);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(new ErrorDetails
            {
                Status = context.Response.StatusCode,
                StatusText = "System Error",
                Message = message
            }.ToString());
        }
    }
}
