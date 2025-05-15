using RestERP.Domain.Entities.Base;

namespace RestERP.Domain.Entities
{
    /// <summary>
    /// Sipariş kalemi entity sınıfı
    /// </summary>
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        // İlişkiler
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
} 