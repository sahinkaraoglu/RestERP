using RestERP.Domain.Entities.Base;

namespace RestERP.Domain.Entities
{
    /// <summary>
    /// Ürün kategorisi entity sınıfı
    /// </summary>
    public class FoodCategory : BaseEntity
    {
        public string Name { get; set; }
        public string TurkishName { get; set; }
        public string? Description { get; set; }
        public ICollection<Food>? SubCategories { get; set; }
    }
} 