using RestERP.Application.Services.Abstract;
using RestERP.Domain.Enums;
using RestERP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int _orderCounter = 0;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // Sipariş numarası oluştur (örnek: ORD-20230814-000001)
            order.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Interlocked.Increment(ref _orderCounter):D6}";
            order.OrderDate = DateTime.UtcNow;
            
            // Order'ı OrderItem'ları ile birlikte tek seferde ekle
            // Entity Framework navigation property sayesinde OrderItem'ları otomatik ekleyecek
            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            
            if (order == null)
                return false;
                
            _unitOfWork.Repository<Order>().Delete(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            // Include ile tek sorguda ilişkili verileri çek - N+1 Query problemi çözümü
            var includes = new List<Expression<Func<Order, object>>>
            {
                o => o.OrderItems
            };
            
            var orders = await _unitOfWork.Repository<Order>().GetAsync(
                predicate: o => true,
                orderBy: q => q.OrderByDescending(o => o.OrderDate),
                includes: includes);
            
            // OrderItems içindeki Food bilgilerini tek sorguda çek
            var orderIds = orders.Select(o => o.Id).ToList();
            var allOrderItems = await _unitOfWork.Repository<OrderItem>().GetAsync(oi => orderIds.Contains(oi.OrderId));
            var foodIds = allOrderItems.Select(oi => oi.FoodId).Distinct().ToList();
            var allFoods = await _unitOfWork.Repository<Food>().GetAsync(f => foodIds.Contains(f.Id));
            var foodDict = allFoods.ToDictionary(f => f.Id);
            
            foreach (var order in orders)
            {
                var items = allOrderItems.Where(oi => oi.OrderId == order.Id).ToList();
                foreach (var item in items)
                {
                    if (foodDict.TryGetValue(item.FoodId, out var food))
                    {
                        item.Food = food;
                    }
                }
                order.OrderItems = items;
            }
            
            return orders;
        }

        public async Task<IEnumerable<Order>> GetActiveOrdersAsync()
        {
            // Include ile tek sorguda ilişkili verileri çek - N+1 Query problemi çözümü
            var includes = new List<Expression<Func<Order, object>>>
            {
                o => o.OrderItems
            };
            
            // Aktif sipariş statüsündeki siparişleri filtreler
            var orders = await _unitOfWork.Repository<Order>().GetAsync(
                predicate: o => (o.Status == OrderStatus.New || 
                                o.Status == OrderStatus.InProgress || 
                                o.Status == OrderStatus.Ready) &&
                                o.Status != OrderStatus.Cancelled,
                orderBy: q => q.OrderByDescending(o => o.OrderDate),
                includes: includes);
            
            // Aktif sipariş kalemlerini filtrele ve toplam tutarı hesapla
            foreach (var order in orders)
            {
                var activeItems = order.OrderItems
                    .Where(oi => oi.Status != OrderStatus.Cancelled)
                    .ToList();
                order.OrderItems = activeItems;
                
                // Toplam tutarı sadece aktif sipariş kalemlerinden hesapla
                order.TotalAmount = activeItems.Sum(oi => oi.TotalPrice);
            }

            return orders.Where(o => o.OrderItems != null && o.OrderItems.Any());
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
                
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetOrderWithDetailsAsync(int id)
        {
            // Include ile tek sorguda ilişkili verileri çek - N+1 Query problemi çözümü
            var includes = new List<Expression<Func<Order, object>>>
            {
                o => o.OrderItems
            };
            
            var orders = await _unitOfWork.Repository<Order>().GetAsync(
                predicate: o => o.Id == id,
                orderBy: q => q.OrderBy(o => o.Id),
                includes: includes);

            var order = orders.FirstOrDefault();
            if (order == null)
                throw new KeyNotFoundException($"Sipariş bulunamadı. Id: {id}");

            // Aktif sipariş kalemlerini filtrele
            var activeItems = order.OrderItems
                .Where(oi => oi.Status != OrderStatus.Cancelled)
                .ToList();
            
            // Food bilgilerini tek sorguda çek
            var foodIds = activeItems.Select(oi => oi.FoodId).Distinct().ToList();
            var foods = await _unitOfWork.Repository<Food>().GetAsync(f => foodIds.Contains(f.Id));
            var foodDict = foods.ToDictionary(f => f.Id);
            
            foreach (var item in activeItems)
            {
                if (foodDict.TryGetValue(item.FoodId, out var food))
                {
                    item.Food = food;
                }
            }
            order.OrderItems = activeItems;

            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime date)
        {
            // Include ile tek sorguda ilişkili verileri çek - N+1 Query problemi çözümü
            var includes = new List<Expression<Func<Order, object>>>
            {
                o => o.OrderItems
            };
            
            var orders = await _unitOfWork.Repository<Order>().GetAsync(
                predicate: o => o.OrderDate.Date == date.Date,
                orderBy: q => q.OrderByDescending(o => o.OrderDate),
                includes: includes);

            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            // Include ile tek sorguda ilişkili verileri çek - N+1 Query problemi çözümü
            var includes = new List<Expression<Func<Order, object>>>
            {
                o => o.OrderItems
            };
            
            var orders = await _unitOfWork.Repository<Order>().GetAsync(
                predicate: o => o.OrderDate.Date >= startDate.Date && o.OrderDate.Date <= endDate.Date,
                orderBy: q => q.OrderByDescending(o => o.OrderDate),
                includes: includes);

            return orders;
        }

        public async Task<bool> DeleteOrderItemAsync(int orderItemId)
        {
            var item = await _unitOfWork.Repository<OrderItem>().GetByIdAsync(orderItemId);
            if (item == null)
                return false;
            
            item.Status = OrderStatus.Cancelled;
            _unitOfWork.Repository<OrderItem>().Update(item);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
} 