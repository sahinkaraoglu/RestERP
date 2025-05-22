using System.Collections.Generic;

namespace RestERP.Web.Models
{
    public class OrderViewModel
    {
        public List<OrderItemViewModel> Items { get; set; } = new();
        public CustomerInfoViewModel CustomerInfo { get; set; } = new();
        public decimal Total { get; set; }
        public int TableNumber { get; set; }
    }

    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class CustomerInfoViewModel
    {
        public string Type { get; set; } = string.Empty;
        public int TableNumber { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
    }
} 