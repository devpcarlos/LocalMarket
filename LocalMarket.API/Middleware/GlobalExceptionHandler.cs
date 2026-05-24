
using LocalMarket.Core.DTos.Common;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace LocalMarket.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, 
            Exception ex, CancellationToken cancellationToken)
        {
            var (statusCode, message) = ex switch
            {
                UnauthorizedAccessException =>
                    (HttpStatusCode.Unauthorized, "Unauthorized"),

                InvalidOperationException =>
                    (HttpStatusCode.BadRequest, ex.Message),

                KeyNotFoundException =>
                    (HttpStatusCode.NotFound, ex.Message),

                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(ex, "Unhandled exception {Method} {Path}",
                    context.Request.Method, context.Request.Path);
            else
                _logger.LogWarning("{Type} {Method} {Path}: {Message}",
                    ex.GetType().Name, context.Request.Method,
                    context.Request.Path, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            
            await context.Response.WriteAsJsonAsync(
                ApiResponseDto<string>.Fail(message),
                cancellationToken
                );

            return true;
        }
    }
}