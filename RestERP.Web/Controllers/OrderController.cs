using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;
using RestERP.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Web.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;

    public OrderController(ILogger<OrderController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    public IActionResult Index()
    {
        // Doğrudan erişim için kullanıcıyı masa sayfasına yönlendir
        if (!Request.Query.ContainsKey("orderData"))
        {
            return View();
        }
        
        return View();
    }

    // Belirli bir masanın siparişlerini görüntülemek için yeni action
    public async Task<IActionResult> ViewOrder(int tableId)
    {
        try
        {
            var orders = await _orderService.GetOrdersByTableIdAsync(tableId);
            var activeOrder = orders.FirstOrDefault(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled);
            
            if (activeOrder == null)
            {
                TempData["Message"] = "Bu masaya ait aktif sipariş bulunmamaktadır.";
                return RedirectToAction("Index", "Table");
            }

            // Sipariş verilerini view'a gönder
            return View("Index", activeOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Masa {tableId} için siparişler alınırken hata oluştu");
            TempData["ErrorMessage"] = "Siparişler alınırken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Index", "Table");
        }
    }

    // Aktif siparişleri listeleyen sayfa
    public async Task<IActionResult> ActiveOrders()
    {
        try
        {
            var activeOrders = await _orderService.GetActiveOrdersAsync();
            return View(activeOrders);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Aktif siparişler listelenirken hata oluştu");
            TempData["ErrorMessage"] = "Aktif siparişler listelenirken bir hata oluştu: " + ex.Message;
            return View("Error");
        }
    }

    [HttpPost]
    [Route("api/orders")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Modeli entity'ye dönüştür
            var order = new Order
            {
                TableId = model.TableNumber,
                Status = OrderStatus.New,
                TotalAmount = model.Total,
                OrderItems = new List<OrderItem>()
            };

            // Sipariş kalemlerini ekle
            foreach (var item in model.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalPrice = item.Price * item.Quantity
                });
            }

            // Siparişi kaydet
            var result = await _orderService.CreateOrderAsync(order);

            return Ok(new { orderId = result.Id, orderNumber = result.OrderNumber });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
            return StatusCode(500, "Sipariş oluşturulurken bir hata oluştu");
        }
    }

    [HttpGet]
    [Route("api/orders/table/{tableId}")]
    public async Task<IActionResult> GetOrdersByTable(int tableId)
    {
        try
        {
            var orders = await _orderService.GetOrdersByTableIdAsync(tableId);
            return Ok(orders);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Masa {tableId} için siparişler alınırken hata oluştu");
            return StatusCode(500, "Siparişler alınırken bir hata oluştu");
        }
    }

    [HttpPut]
    [Route("api/orders/{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateModel model)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Sipariş bulunamadı");
            }

            // Status string'ini enum'a çevir
            if (Enum.TryParse<OrderStatus>(model.Status, out OrderStatus newStatus))
            {
                order.Status = newStatus;
                await _orderService.UpdateOrderAsync(order);
                return Ok();
            }
            
            return BadRequest("Geçersiz sipariş durumu");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Sipariş durumu güncellenirken hata oluştu. OrderId: {orderId}");
            return StatusCode(500, "Sipariş durumu güncellenirken bir hata oluştu");
        }
    }
}

// View Models
public class OrderViewModel
{
    public int TableNumber { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
}

public class OrderItemViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class OrderStatusUpdateModel
{
    public string Status { get; set; } = string.Empty;
} 