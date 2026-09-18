namespace RestERP.Core.Interfaces.Logging
{
    public interface IRequestLogWriter
    {
        Task WriteAsync(RequestLogEntry entry, CancellationToken cancellationToken = default);
    }

    public record RequestLogEntry(
        string RequestName,
        string RequestType,
        string? RequestPayload,
        string? ResponsePayload,
        bool IsSuccess,
        string? ErrorMessage,
        string? StackTrace,
        long ElapsedMs,
        string? UserId,
        string? UserName,
        string? CorrelationId);
}
