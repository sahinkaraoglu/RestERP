using RestERP.Core.Domain.Entities.Base;
using RestERP.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestERP.Core.Domain.Entities
{
    /// <summary>
    /// Sipariş kalemi entity sınıfı
    /// </summary>
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        
        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; } = false;

        public OrderStatus Status { get; set; } = OrderStatus.New;

        // İlişkiler
        public Order Order { get; set; } = null!;
        public Food Food { get; set; } = null!;
    }
} 