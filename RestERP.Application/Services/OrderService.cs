using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Enums;
using RestERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RestERP.Core.Domain.Entities;

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

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            
            if (order == null)
                return false;
                
            await _unitOfWork.Repository<Order>().DeleteAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            foreach (var order in orders)
            {
                var orderItems = await _unitOfWork.Repository<OrderItem>().GetAsync(oi => oi.OrderId == order.Id);
                foreach (var item in orderItems)
                {
                    item.Food = await _unitOfWork.Repository<Food>().GetByIdAsync(item.FoodId);
                }
                order.OrderItems = orderItems.ToList();
            }
            return orders;
        }

        public async Task<IEnumerable<Order>> GetActiveOrdersAsync()
        {
            // Aktif sipariş statüsündeki siparişleri filtreler
            // New, InProgress, Ready statüsündeki siparişler aktif kabul edilir
            var orders = await _unitOfWork.Repository<Order>().GetAsync(o => 
                (o.Status == OrderStatus.New || 
                o.Status == OrderStatus.InProgress || 
                o.Status == OrderStatus.Ready) &&
                !o.IsDeleted);

            // Her sipariş için sipariş kalemlerini yükle
            foreach (var order in orders)
            {
                var orderItems = await _unitOfWork.Repository<OrderItem>()
                    .GetAsync(oi => oi.OrderId == order.Id && !oi.IsDeleted);
                order.OrderItems = orderItems.ToList();
            }

            return orders;
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

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
                
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetOrderWithDetailsAsync(int id)
        {
            var orders = await _unitOfWork.Repository<Order>().GetAsync(o => o.Id == id);

            var order = orders.FirstOrDefault();
            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. Id: {id}");

            // Sipariş kalemlerini ve Food bilgisini yükle
            var orderItems = await _unitOfWork.Repository<OrderItem>().GetAsync(oi => oi.OrderId == id);
            foreach (var item in orderItems)
            {
                item.Food = await _unitOfWork.Repository<Food>().GetByIdAsync(item.FoodId);
            }
            order.OrderItems = orderItems.ToList();

            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date)
        {
            var orders = await _unitOfWork.Repository<Order>().GetAsync(o => 
                o.OrderDate.Date == date.Date && 
                !o.IsDeleted);

            foreach (var order in orders)
            {
                var orderItems = await _unitOfWork.Repository<OrderItem>()
                    .GetAsync(oi => oi.OrderId == order.Id && !oi.IsDeleted);
                order.OrderItems = orderItems.ToList();
            }

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _unitOfWork.Repository<Order>().GetAsync(o => 
                o.OrderDate.Date >= startDate.Date && 
                o.OrderDate.Date <= endDate.Date && 
                !o.IsDeleted);

            foreach (var order in orders)
            {
                var orderItems = await _unitOfWork.Repository<OrderItem>()
                    .GetAsync(oi => oi.OrderId == order.Id && !oi.IsDeleted);
                order.OrderItems = orderItems.ToList();
            }

            return orders;
        }

        public async Task<bool> DeleteOrderItemAsync(int orderItemId)
        {
            var item = await _unitOfWork.Repository<OrderItem>().GetByIdAsync(orderItemId);
            if (item == null)
                return false;
            await _unitOfWork.Repository<OrderItem>().DeleteAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
} 