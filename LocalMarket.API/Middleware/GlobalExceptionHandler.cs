
using LocalMarket.Core.DTos.Common;
using LocalMarket.Core.Exception;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var (statusCode, title, type) = exception switch
            {
                EmailNotFoundException =>
                    (HttpStatusCode.NotFound, "Email not found",
                    "https://localmarket.com/email-not-found"),

                UnauthorizedAccessException =>
                    (HttpStatusCode.Unauthorized, "Unauthorized",
                    "https://localmarket.com/unauthorized"),

                InvalidOperationException =>
                    (HttpStatusCode.BadRequest, "Invalid operation",
                    "https://localmarket.com/invalid-operation"),

                KeyNotFoundException =>
                    (HttpStatusCode.NotFound, "Key not found",
                    "https://localmarket.com/key-not-found"),

                    ArgumentException or ArgumentNullException =>(HttpStatusCode
                    .BadRequest, "Bad request", "https://localmarket.com/bad-request"),

                _ => (HttpStatusCode.InternalServerError, "Internal server error",
                    "https://localmarket.com/internal-server-error")

            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(exception, "Unhandled exception {Method} {Path}",
                    httpContext.Request.Method, httpContext.Request.Path);
            else
                _logger.LogWarning("{Type} {Method} {Path}: {Message}",
                    exception.GetType().Name, httpContext.Request.Method,
                    httpContext.Request.Path, exception.Message);

            var problem = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Type = type,
                Detail = statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred. Please try again later."
                    : exception.Message,
                    Instance = httpContext.Request.Path
            };
            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }
    }
}