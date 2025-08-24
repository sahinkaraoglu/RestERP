using RestERP.Core.Domain.Entities.Base;
using RestERP.Domain.Enums;

namespace RestERP.Core.Domain.Entities
{
    /// <summary>
    /// Sipariş entity sınıfı
    /// </summary>
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int? TableId { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.New;
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        
        // İlişkiler
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
} 