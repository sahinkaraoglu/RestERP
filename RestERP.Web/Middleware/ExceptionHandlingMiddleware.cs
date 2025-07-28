using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestERP.Application.Interfaces;

namespace RestERP.Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, "Kaynak bulunamadı", "KeyNotFoundException");
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "Geçersiz argüman", "ArgumentException");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Sunucu hatası", "Exception");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string message, string exceptionType)
        {
            _logger.LogError(exception, exception.Message);

            // Log'u veritabanına kaydet
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
                
                var userId = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null;
                var additionalData = JsonSerializer.Serialize(new
                {
                    StatusCode = (int)statusCode,
                    ExceptionType = exceptionType,
                    RequestPath = context.Request.Path,
                    RequestMethod = context.Request.Method
                });

                await logService.LogErrorAsync(exception, "ExceptionHandlingMiddleware", userId, additionalData);
            }
            catch (Exception logException)
            {
                _logger.LogError(logException, "Log kaydetme sırasında hata oluştu");
            }

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
    }
} 