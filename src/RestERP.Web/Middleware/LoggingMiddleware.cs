using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestERP.Core.Domain.Entities;
using RestERP.Infrastructure.Context;
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

        public async Task InvokeAsync(HttpContext context, RestERPDbContext dbContext)
        {
            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;

            try
            {
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                await _next(context);

                stopwatch.Stop();
                await LogRequest(context, dbContext, stopwatch.ElapsedMilliseconds, null);

                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                await LogRequest(context, dbContext, stopwatch.ElapsedMilliseconds, ex);
                throw;
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private async Task LogRequest(HttpContext context, RestERPDbContext dbContext, long elapsedMs, Exception exception = null)
        {
            try
            {
                var log = new Log
                {
                    Level = exception != null ? "Error" : "Information",
                    Message = exception?.Message ?? $"Request completed in {elapsedMs}ms",
                    Exception = exception?.ToString(),
                    StackTrace = exception?.StackTrace,
                    Source = context.Request.Path,
                    UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    UserName = context.User?.FindFirst(ClaimTypes.Name)?.Value,
                    RequestPath = context.Request.Path,
                    RequestMethod = context.Request.Method,
                    IpAddress = GetClientIpAddress(context),
                    Timestamp = DateTime.UtcNow
                };

                dbContext.Logs.Add(log);
                await dbContext.SaveChangesAsync();

                // Console'a da log yazdır
                if (exception != null)
                {
                    _logger.LogError(exception, "Request failed: {Path} - {Message}", context.Request.Path, exception.Message);
                }
                else
                {
                    _logger.LogInformation("Request completed: {Path} - {ElapsedMs}ms", context.Request.Path, elapsedMs);
                }
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Error logging request to database");
            }
        }

        private string GetClientIpAddress(HttpContext context)
        {
            var forwardedHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                return forwardedHeader.Split(',')[0].Trim();
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
} 