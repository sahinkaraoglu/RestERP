using System.Net;
using System.Text.Json;
using FluentValidation;

namespace RestERP.API.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, "Kaynak bulunamadı");
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "Geçersiz argüman");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Sunucu hatası");
            }
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            _logger.LogWarning("Validation failed: {Errors}",
                string.Join(", ", exception.Errors.Select(e => e.ErrorMessage)));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Doğrulama hatası",
                Errors = exception.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                })
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            HttpStatusCode statusCode,
            string message)
        {
            _logger.LogError(exception, exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Detail = exception.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
