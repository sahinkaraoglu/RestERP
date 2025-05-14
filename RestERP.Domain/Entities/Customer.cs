using MarketWorld.Domain.Entities.Base;

namespace RestERP.Domain.Entities
{
    /// <summary>
    /// Müşteri entity sınıfı
    /// </summary>
    public class Customer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 