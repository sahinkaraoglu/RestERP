using RestERP.Core.Doman.Entities.Base;

namespace RestERP.Core.Doman.Entities
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