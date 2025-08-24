using RestERP.Core.Domain.Entities.Base;

namespace RestERP.Core.Domain.Entities
{
    /// <summary>
    /// Ürün kategorisi entity sınıfı
    /// </summary>
    public class FoodCategory : BaseEntity
    {
        public string Name { get; set; }
        public string TurkishName { get; set; }
        public string? Description { get; set; }
    }
} 