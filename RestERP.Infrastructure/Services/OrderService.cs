using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestERP.Domain.Entities;
using RestERP.Domain.Enums;
using RestERP.Application.Services.Interfaces;
using RestERP.Infrastructure.Data;

namespace RestERP.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly RestERPDbContext _context;

        public OrderService(RestERPDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // Sipariş numarası oluştur
            order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000):000}";
            
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            order.IsDeleted = true;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetActiveOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => (o.Status == OrderStatus.New || 
                           o.Status == OrderStatus.InProgress || 
                           o.Status == OrderStatus.Ready) && 
                           !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
                ?? throw new KeyNotFoundException($"Sipariş bulunamadı. Id: {id}");
        }

        public async Task<IEnumerable<Order>> GetOrdersByTableIdAsync(int tableId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.TableId == tableId && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Food)
                .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);

            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı: {orderId}");

            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 