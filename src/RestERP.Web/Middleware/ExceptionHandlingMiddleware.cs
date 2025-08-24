using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestERP.Core.Domain.Entities;
using RestERP.Infrastructure.Context;
using System.Security.Claims;

namespace RestERP.Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RestERPDbContext dbContext)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, dbContext, ex, HttpStatusCode.NotFound, "Kaynak bulunamadı");
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, dbContext, ex, HttpStatusCode.BadRequest, "Geçersiz argüman");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, dbContext, ex, HttpStatusCode.InternalServerError, "Sunucu hatası");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, RestERPDbContext dbContext, Exception exception, HttpStatusCode statusCode, string message)
        {
            _logger.LogError(exception, exception.Message);

            // Hatayı veritabanına logla
            await LogExceptionToDatabase(context, dbContext, exception, statusCode, message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new 
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Detail = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }

        private async Task LogExceptionToDatabase(HttpContext context, RestERPDbContext dbContext, Exception exception, HttpStatusCode statusCode, string message)
        {
            try
            {
                var log = new Log
                {
                    Level = "Error",
                    Message = $"{message}: {exception.Message}",
                    Exception = exception.ToString(),
                    StackTrace = exception.StackTrace,
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
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Error logging exception to database");
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