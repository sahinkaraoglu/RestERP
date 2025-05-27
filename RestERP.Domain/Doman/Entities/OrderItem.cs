using RestERP.Core.Doman.Entities.Base;

namespace RestERP.Core.Doman.Entities
{
    /// <summary>
    /// Sipariş kalemi entity sınıfı
    /// </summary>
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        // İlişkiler
        public Order Order { get; set; } = null!;
        public Food Food { get; set; } = null!;
    }
} 