namespace RestERP.Core.Domain.Entities
{
    public class RequestLog
    {
        public long Id { get; set; }
        public string RequestName { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public string? RequestPayload { get; set; }
        public string? ResponsePayload { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }
        public long ElapsedMs { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? CorrelationId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
