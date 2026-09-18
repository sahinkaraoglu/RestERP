using System.Diagnostics;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestERP.Application.Logging;
using RestERP.Core.Interfaces.Logging;

namespace RestERP.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IRequestLogWriter _logWriter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(
            IRequestLogWriter logWriter,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logWriter = logWriter;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var skipPayload = request is INoLogRequest;
            var requestName = typeof(TRequest).Name;
            var requestType = requestName.EndsWith("Query", StringComparison.Ordinal) ? "Query" : "Command";
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = user?.Identity?.Name;
            var correlationId = Activity.Current?.Id
                ?? _httpContextAccessor.HttpContext?.TraceIdentifier;

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next();
                stopwatch.Stop();

                await SafeWriteLogAsync(new RequestLogEntry(
                    requestName,
                    requestType,
                    skipPayload ? null : RequestLogSerializer.Serialize(request),
                    skipPayload ? null : RequestLogSerializer.Serialize(response),
                    true,
                    null,
                    null,
                    stopwatch.ElapsedMilliseconds,
                    userId,
                    userName,
                    correlationId), cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                await SafeWriteLogAsync(new RequestLogEntry(
                    requestName,
                    requestType,
                    skipPayload ? null : RequestLogSerializer.Serialize(request),
                    null,
                    false,
                    ex.Message,
                    ex.StackTrace,
                    stopwatch.ElapsedMilliseconds,
                    userId,
                    userName,
                    correlationId), cancellationToken);

                throw;
            }
        }

        private async Task SafeWriteLogAsync(RequestLogEntry entry, CancellationToken cancellationToken)
        {
            try
            {
                await _logWriter.WriteAsync(entry, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request log yazılamadı: {RequestName}", entry.RequestName);
            }
        }
    }
}
