using RestERP.Core.Domain.Entities.Base;

namespace RestERP.Core.Domain.Entities
{
    public class Log : BaseEntity
    {
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RequestPath { get; set; }
        public string RequestMethod { get; set; }
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 