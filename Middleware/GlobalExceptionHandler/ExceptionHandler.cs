using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Middleware.GlobalExceptionHandler
{
    public class ExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        private static object HandleException(Exception exception, ILogger logger)
        {
            logger.LogError(exception, "An unhandled exception occurred.");
            return new
            {
                Success = false,
                Message = "An internal server error occurred.",
                Error = exception.Message
            };
        }

        public static async Task WriteErrorResponseAsync(HttpContext context, Exception exception, ILogger logger)
        {
            logger.LogError(exception, "An error occurred while processing the request.");

            var errorResponse = new
            {
                Success = false,
                Message = "An internal server error occurred.",
                Error = exception.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
