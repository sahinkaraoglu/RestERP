using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;
using RestERP.Domain.Enums;
using RestERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RestERP.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // Sipariş numarası oluştur (örnek: ORD-20230814-001)
            order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000):000}";
            
            await _unitOfWork.Repository<Order>().AddAsync(order);
            
            // Sipariş kalemleri için
            foreach (var item in order.OrderItems)
            {
                item.OrderId = order.Id;
                await _unitOfWork.Repository<OrderItem>().AddAsync(item);
            }
            
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            
            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. Id: {id}");
                
            await _unitOfWork.Repository<Order>().DeleteAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _unitOfWork.Repository<Order>().GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetActiveOrdersAsync()
        {
            // Aktif sipariş statüsündeki siparişleri filtreler
            // New, InProgress, Ready statüsündeki siparişler aktif kabul edilir
            return await _unitOfWork.Repository<Order>().GetAsync(o => 
                o.Status == OrderStatus.New || 
                o.Status == OrderStatus.InProgress || 
                o.Status == OrderStatus.Ready);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            
            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. Id: {id}");
                
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByTableIdAsync(int tableId)
        {
            return await _unitOfWork.Repository<Order>().GetAsync(o => o.TableId == tableId);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
                
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Order> GetOrderWithDetailsAsync(int id)
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetAsync(o => o.Id == id);

            var order = orders.FirstOrDefault();
            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. Id: {id}");

            // Sipariş kalemlerini ayrıca yükle
            var orderItems = await _unitOfWork.Repository<OrderItem>()
                .GetAsync(oi => oi.OrderId == id);

            order.OrderItems = orderItems.ToList();

            return order;
        }
    }
} 