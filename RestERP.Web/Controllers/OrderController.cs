using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;
using RestERP.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using RestERP.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace RestERP.Web.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;
    private readonly IFoodService _foodService;

    public OrderController(
        ILogger<OrderController> logger, 
        IOrderService orderService,
        IFoodService foodService)
    {
        _logger = logger;
        _orderService = orderService;
        _foodService = foodService;
    }

    public async Task<IActionResult> Index(int? orderId = null)
    {
        try
        {
            if (orderId == null)
            {
                return View(null);
            }

            var order = await _orderService.GetOrderWithDetailsAsync(orderId.Value);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                return View(null);
            }

            // Her bir sipariş kalemi için ürün bilgilerini yükle
            foreach (var item in order.OrderItems)
            {
                item.Food = await _foodService.GetFoodByIdAsync(item.ProductId);
            }

            return View(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş sayfası yüklenirken hata oluştu");
            return View(null);
        }
    }

    // Belirli bir masanın siparişlerini görüntülemek için action
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

            // Sipariş detaylarını getir
            var orderWithDetails = await _orderService.GetOrderWithDetailsAsync(activeOrder.Id);
            
            // Her bir sipariş kalemi için ürün bilgilerini yükle
            foreach (var item in orderWithDetails.OrderItems)
            {
                item.Food = await _foodService.GetFoodByIdAsync(item.ProductId);
            }

            // Sipariş verilerini view'a gönder
            return View("Index", orderWithDetails);
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
            foreach (var order in activeOrders)
            {
                // Her bir sipariş kalemi için yemek bilgilerini yükle
                foreach (var item in order.OrderItems)
                {
                    item.Food = await _foodService.GetFoodByIdAsync(item.ProductId);
                }
            }
            return View(activeOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Aktif siparişler listelenirken hata oluştu");
            TempData["ErrorMessage"] = "Aktif siparişler listelenirken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Index", "Table");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sipariş oluştur
            var order = new Order
            {
                TableId = model.CustomerInfo.Type == "dine-in" ? model.CustomerInfo.TableNumber : 0,
                Status = OrderStatus.New,
                TotalAmount = model.Items.Sum(i => i.Price * i.Quantity),
                OrderItems = model.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Price,
                    TotalPrice = i.Price * i.Quantity
                }).ToList()
            };

            // Siparişi kaydet
            var result = await _orderService.CreateOrderAsync(order);

            // Başarılı sonuç dön
            return Json(new { success = true, orderId = result.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
            return Json(new { success = false, message = "Sipariş oluşturulurken bir hata oluştu: " + ex.Message });
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

public class OrderStatusUpdateModel
{
    public string Status { get; set; } = string.Empty;
} 