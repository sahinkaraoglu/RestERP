using System.Text.Json;
using Microsoft.AspNetCore.Http;
using RestERP.Application.Interfaces;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces;

namespace RestERP.Application.Services
{
    public class LogService : ILogService
    {
        private readonly IRepository<Log> _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(IRepository<Log> logRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogErrorAsync(Exception exception, string source, string userId = null, string additionalData = null)
        {
            var log = CreateLogEntry("Error", exception.Message, source, userId, additionalData);
            log.Exception = exception.ToString();
            log.StackTrace = exception.StackTrace;
            
            await SaveLogAsync(log);
        }

        public async Task LogWarningAsync(string message, string source, string userId = null, string additionalData = null)
        {
            var log = CreateLogEntry("Warning", message, source, userId, additionalData);
            await SaveLogAsync(log);
        }

        public async Task LogInfoAsync(string message, string source, string userId = null, string additionalData = null)
        {
            var log = CreateLogEntry("Info", message, source, userId, additionalData);
            await SaveLogAsync(log);
        }

        public async Task LogDebugAsync(string message, string source, string userId = null, string additionalData = null)
        {
            var log = CreateLogEntry("Debug", message, source, userId, additionalData);
            await SaveLogAsync(log);
        }

        public async Task<IEnumerable<Log>> GetLogsAsync(DateTime? startDate = null, DateTime? endDate = null, string level = null, string source = null)
        {
            var allLogs = await _logRepository.GetAllAsync();
            var query = allLogs.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(l => l.CreatedDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(l => l.CreatedDate <= endDate.Value);

            if (!string.IsNullOrEmpty(level))
                query = query.Where(l => l.Level == level);

            if (!string.IsNullOrEmpty(source))
                query = query.Where(l => l.Source == source);

            return query.OrderByDescending(l => l.CreatedDate).ToList();
        }

        public async Task<Log> GetLogByIdAsync(int id)
        {
            return await _logRepository.GetByIdAsync(id) ?? throw new ArgumentException($"Log with id {id} not found");
        }

        private Log CreateLogEntry(string level, string message, string source, string userId, string additionalData)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            
            return new Log
            {
                Level = level,
                Message = message,
                Source = source,
                UserId = userId,
                AdditionalData = additionalData,
                UserAgent = httpContext?.Request.Headers["User-Agent"].ToString(),
                IpAddress = GetClientIpAddress(httpContext),
                RequestPath = httpContext?.Request.Path,
                RequestMethod = httpContext?.Request.Method,
                CreatedDate = DateTime.UtcNow
            };
        }

        private async Task SaveLogAsync(Log log)
        {
            await _logRepository.AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        private string GetClientIpAddress(HttpContext httpContext)
        {
            if (httpContext == null) return string.Empty;

            // X-Forwarded-For header'ı kontrol et (proxy arkasında çalışıyorsa)
            var forwardedHeader = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                return forwardedHeader.Split(',')[0].Trim();
            }

            // X-Real-IP header'ı kontrol et
            var realIpHeader = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIpHeader))
            {
                return realIpHeader;
            }

            // Remote IP address
            return httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }
    }
} 