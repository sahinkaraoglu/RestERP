using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestERP.Domain.Enums;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByTableIdAsync(int tableId);
        Task<IEnumerable<Order>> GetActiveOrdersAsync();
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> DeleteOrderItemAsync(int orderItemId);
    }
} 