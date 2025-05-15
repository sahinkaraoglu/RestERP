using RestERP.Domain.Entities.Base;

namespace RestERP.Domain.Entities
{
    /// <summary>
    /// Ürün kategorisi entity sınıfı
    /// </summary>
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 