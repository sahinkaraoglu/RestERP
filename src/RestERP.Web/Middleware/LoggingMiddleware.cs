using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace RestERP.Web.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;

            try
            {
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                await _next(context);

                stopwatch.Stop();
                await LogRequest(context, stopwatch.ElapsedMilliseconds, null);

                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                await LogRequest(context, stopwatch.ElapsedMilliseconds, ex);
                throw;
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private Task LogRequest(HttpContext context, long elapsedMs, Exception exception = null)
        {
            try
            {
                // Console/structured logger
                if (exception != null)
                {
                    _logger.LogError(exception, "Request failed: {Path} {Method} {ElapsedMs}ms User:{UserId}",
                        context.Request.Path,
                        context.Request.Method,
                        elapsedMs,
                        context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                else
                {
                    _logger.LogInformation("Request completed: {Path} {Method} {ElapsedMs}ms User:{UserId}",
                        context.Request.Path,
                        context.Request.Method,
                        elapsedMs,
                        context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Error while logging request");
            }

            return Task.CompletedTask;
        }
    }
} 