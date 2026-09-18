using RestERP.Core.Interfaces.Logging;
using RestERP.Core.Domain.Entities;
using RestERP.Infrastructure.Context;

namespace RestERP.Infrastructure.Logging
{
    public class RequestLogWriter : IRequestLogWriter
    {
        private readonly LoggingDbContext _context;

        public RequestLogWriter(LoggingDbContext context)
        {
            _context = context;
        }

        public async Task WriteAsync(RequestLogEntry entry, CancellationToken cancellationToken = default)
        {
            _context.RequestLogs.Add(new RequestLog
            {
                RequestName = entry.RequestName,
                RequestType = entry.RequestType,
                RequestPayload = entry.RequestPayload,
                ResponsePayload = entry.ResponsePayload,
                IsSuccess = entry.IsSuccess,
                ErrorMessage = entry.ErrorMessage,
                StackTrace = entry.StackTrace,
                ElapsedMs = entry.ElapsedMs,
                UserId = entry.UserId,
                UserName = entry.UserName,
                CorrelationId = entry.CorrelationId,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
