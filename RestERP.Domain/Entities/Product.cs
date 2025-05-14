using RestERP.Domain.Common;

namespace RestERP.Domain.Entities
{
    /// <summary>
    /// Ürün entity sınıfı
    /// </summary>
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int? CategoryId { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 