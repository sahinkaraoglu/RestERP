using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Interfaces
{
    public interface ILogService
    {
        Task LogErrorAsync(Exception exception, string source, string userId = null, string additionalData = null);
        Task LogWarningAsync(string message, string source, string userId = null, string additionalData = null);
        Task LogInfoAsync(string message, string source, string userId = null, string additionalData = null);
        Task LogDebugAsync(string message, string source, string userId = null, string additionalData = null);
        Task<IEnumerable<Log>> GetLogsAsync(DateTime? startDate = null, DateTime? endDate = null, string level = null, string source = null);
        Task<Log> GetLogByIdAsync(int id);
    }
} 