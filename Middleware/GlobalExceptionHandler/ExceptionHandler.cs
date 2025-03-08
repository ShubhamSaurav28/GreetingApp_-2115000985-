using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        private static string HandleException(Exception exception,ILogger logger,out object errorResponse)
        {
            logger.LogError("An error occurred in the application");
            errorResponse = new 
            {
                Success = false,
                Message = "An error occurred",
                Error = exception.Message()
            }
            return JsonConvert.SerializeObject(errorResponse);
        }
    }
}
