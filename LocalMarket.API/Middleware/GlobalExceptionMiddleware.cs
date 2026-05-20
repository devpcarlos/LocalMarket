using LocalMarket.Core.DTos.Common;
using Supabase.Gotrue.Exceptions;
using Supabase.Postgrest.Exceptions;
using System.Net;
using System.Text.Json;

namespace LocalMarket.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, message) = ex switch
            {
                UnauthorizedAccessException =>
                    (HttpStatusCode.Unauthorized, "Unauthorized"),

                GotrueException gex =>
                    (HttpStatusCode.BadRequest, ExtractGotrueMessage(gex)),

                PostgrestException pgex =>
                    (HttpStatusCode.BadRequest, ExtractPostgrestMessage(pgex)),

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

            var json = JsonSerializer.Serialize(
                ApiResponseDto<string?>.Fail(message),
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            await context.Response.WriteAsync(json);
        }

        private static string ExtractGotrueMessage(GotrueException ex)
        {
            try
            {
                var doc = JsonDocument.Parse(ex.Message);
                if (doc.RootElement.TryGetProperty("msg", out var msg))
                    return msg.GetString() ?? ex.Message;
                if (doc.RootElement.TryGetProperty("error_code", out var code))
                    return code.GetString() ?? ex.Message;
            }
            catch { }
            return ex.Message;
        }

        private static string ExtractPostgrestMessage(PostgrestException ex)
        {
            try
            {
                var doc = JsonDocument.Parse(ex.Message);
                if (doc.RootElement.TryGetProperty("message", out var msg))
                    return msg.GetString() ?? ex.Message;
            }
            catch { }
            return ex.Message;
        }
    }
}