using RestERP.Core.Domain.Entities.Base;

namespace RestERP.Core.Domain.Entities
{
    public class Log : BaseEntity
    {
        public string Level { get; set; } // Error, Warning, Info, Debug
        public string Message { get; set; }
        public string Exception { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; } // Controller, Service, etc.
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
        public string RequestPath { get; set; }
        public string RequestMethod { get; set; }
        public string UserId { get; set; }
        public string AdditionalData { get; set; } // JSON formatında ek bilgiler
    }
} 