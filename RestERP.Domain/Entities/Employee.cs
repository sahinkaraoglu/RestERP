using MarketWorld.Domain.Entities.Base;
using RestERP.Domain.Enums;

namespace RestERP.Domain.Entities
{
    /// <summary>
    /// Çalışan entity sınıfı
    /// </summary>
    public class Employee : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public EmployeeRole Role { get; set; } = EmployeeRole.Waiter;
        public bool IsActive { get; set; } = true;
        public EmployeeRole EmployeeRole { get; set; } 
    }
} 