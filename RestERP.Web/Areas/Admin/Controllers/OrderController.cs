using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestERP.Core.Domain.Entities;
using RestERP.Web.Areas.Admin.Models;

namespace RestERP.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOrderService _orderService;
    private readonly IFoodService _foodService;
    private readonly IUserService _userService;

    public OrderController(
        ILogger<OrderController> logger, 
        IOrderService orderService,
        IFoodService foodService,
        IUserService userService)
    {
        _logger = logger;
        _orderService = orderService;
        _foodService = foodService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "AccessDenied", new { area = "" });
        }
        try
        {
            // Kullanıcı girişi kontrolü
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return View(new List<Order>());
            }

            // Eğer sipariş ID'si belirtilmişse, o siparişin detaylarını göster
            if (Request.Query.ContainsKey("orderId"))
            {
                var orderId = int.TryParse(Request.Query["orderId"], out int id) ? id : (int?)null;
                if (orderId.HasValue)
                {
                    var order = await _orderService.GetOrderWithDetailsAsync(orderId.Value);
                    if (order == null)
                    {
                        TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Her bir sipariş kalemi için ürün bilgilerini yükle
                    foreach (var item in order.OrderItems)
                    {
                        item.Food = await _foodService.GetFoodByIdAsync(item.FoodId);
                    }

                    ViewData["ShowSingleOrder"] = true;
                    return View(new List<Order> { order });
                }
            }
            
            // Sipariş ID'si belirtilmemişse tüm aktif siparişleri göster
            var activeOrders = await _orderService.GetActiveOrdersAsync();
            
            // Eğer aktif sipariş yoksa boş bir liste ile view'ı döndür
            if (!activeOrders.Any())
            {
                ViewData["ShowSingleOrder"] = false;
                return View(new List<Order>());
            }

            // Her bir sipariş için yemek bilgilerini yükle
            foreach (var order in activeOrders)
            {
                foreach (var item in order.OrderItems)
                {
                    try
                    {
                        item.Food = await _foodService.GetFoodByIdAsync(item.FoodId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Yemek bilgisi yüklenirken hata oluştu. FoodId: {item.FoodId}");
                    }
                }
            }

            ViewData["ShowSingleOrder"] = false;
            return View(activeOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş sayfası yüklenirken hata oluştu");
            TempData["ErrorMessage"] = "Sipariş yüklenirken bir hata oluştu.";
            return View(new List<Order>());
        }
    }

    // Belirli bir masanın siparişlerini görüntülemek için action
    public async Task<IActionResult> ViewOrder(int tableId)
    {
        try
        {
            var orders = await _orderService.GetOrdersByTableIdAsync(tableId);
            var activeOrder = orders.FirstOrDefault(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled && !o.IsPaid);
            
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
                item.Food = await _foodService.GetFoodByIdAsync(item.FoodId);
            }

            ViewData["ShowSingleOrder"] = true;
            // Tekil siparişi liste olarak dön
            return View("Index", new List<Order> { orderWithDetails });
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
            
            // Eğer aktif sipariş yoksa boş bir liste ile view'ı döndür
            if (!activeOrders.Any())
            {
                ViewData["ShowSingleOrder"] = false;
                return View("Index", new List<Order>());
            }

            // Her bir sipariş için yemek bilgilerini yükle
            foreach (var order in activeOrders)
            {
                foreach (var item in order.OrderItems)
                {
                    try
                    {
                        item.Food = await _foodService.GetFoodByIdAsync(item.FoodId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Yemek bilgisi yüklenirken hata oluştu. FoodId: {item.FoodId}");
                    }
                }
            }

            ViewData["ShowSingleOrder"] = false;
            return View("Index", activeOrders.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Aktif siparişler listelenirken hata oluştu");
            TempData["ErrorMessage"] = "Aktif siparişler listelenirken bir hata oluştu: " + ex.Message;
            return View("Index", new List<Order>());
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
                    FoodId = i.FoodId,
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
        catch (Exception ex)
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
            if (Enum.TryParse(model.Status, out OrderStatus newStatus))
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

    [HttpGet]
    [Route("api/orders/all-tables")]
    public async Task<IActionResult> GetAllTablesOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var tableOrders = orders
                .Where(o => o.TableId.HasValue)
                .GroupBy(o => o.TableId)
                .Select(g => new
                {
                    TableId = g.Key,
                    Orders = g.Select(o => new
                    {
                        o.Id,
                        o.OrderNumber,
                        o.OrderDate,
                        o.Status,
                        o.TotalAmount,
                        o.IsPaid,
                        Items = o.OrderItems.Select(i => new
                        {
                            i.FoodId,
                            i.Quantity,
                            i.UnitPrice,
                            i.TotalPrice
                        })
                    })
                })
                .ToList();

            return Ok(tableOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tüm masaların siparişleri alınırken hata oluştu");
            return StatusCode(500, "Siparişler alınırken bir hata oluştu");
        }
    }

    [HttpPost]
    [Route("api/orders/checkout/{tableId}")]
    public async Task<IActionResult> CheckoutTable(int tableId)
    {
        try
        {
            // Masaya ait aktif siparişleri bul
            var orders = await _orderService.GetOrdersByTableIdAsync(tableId);
            var activeOrders = orders.Where(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled && !o.IsPaid).ToList();

            if (!activeOrders.Any())
            {
                return BadRequest(new { success = false, message = "Bu masada kapatılacak aktif bir hesap yok." });
            }

            foreach (var order in activeOrders)
            {
                order.IsPaid = true;
                order.Status = OrderStatus.Completed;
                // OrderItem'ların da IsPaid'ini true yap
                foreach (var item in order.OrderItems)
                {
                    item.IsPaid = true;
                }
                await _orderService.UpdateOrderAsync(order);
            }

            return Ok(new { success = true, message = "Hesap kapatıldı" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Masa {tableId} için hesap kapatma işlemi sırasında hata oluştu");
            return StatusCode(500, new { success = false, message = "Hesap kapatılırken bir hata oluştu." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create(int tableId)
    {
        try
        {
            var categories = await _foodService.GetAllFoodCategoriesAsync();
            var foods = await _foodService.GetAllFoodsAsync();
            var images = await _foodService.GetAllFoodImagesAsync();

            ViewBag.Categories = categories;
            ViewBag.Foods = foods;
            ViewBag.Images = images;

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş oluşturma sayfası açılırken hata oluştu");
            TempData["ErrorMessage"] = "Sipariş oluşturma sayfası açılırken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Index", "Table");
        }
    }

    [HttpGet]
    public async Task<IActionResult> CancelOrder(int id)
    {
        try
        {
            var order = await _orderService.GetOrderWithDetailsAsync(id);
            if (order == null)
            {
                return NotFound("Sipariş bulunamadı.");
            }
            return View(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Sipariş iptal sayfası açılırken hata oluştu. OrderId: {id}");
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrder(int id, bool cancelAll = false, int? tableNumber = null)
    {
        try
        {
            var order = await _orderService.GetOrderWithDetailsAsync(id);
            if (order == null)
            {
                return Json(new { success = false, message = "Sipariş bulunamadı." });
            }

            // Tüm sipariş öğelerinin durumunu Cancelled olarak güncelle
            foreach (var item in order.OrderItems)
            {
                item.Status = OrderStatus.Cancelled;
            }

            // Siparişin kendisini de iptal et
            order.Status = OrderStatus.Cancelled;
            
            await _orderService.UpdateOrderAsync(order);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Sipariş iptal edilirken hata oluştu. OrderId: {id}");
            return Json(new { success = false, message = "Sipariş iptal edilirken bir hata oluştu." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CancelTableOrders(int tableNumber)
    {
        try
        {
            var orders = await _orderService.GetOrdersByTableIdAsync(tableNumber);
            var activeOrders = orders.Where(o => o.Status != OrderStatus.Cancelled);

            foreach (var order in activeOrders)
            {
                await _orderService.UpdateOrderStatusAsync(order.Id, OrderStatus.Cancelled);
            }

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Masa {tableNumber} siparişleri iptal edilirken hata oluştu");
            return Json(new { success = false, message = "Siparişler iptal edilirken bir hata oluştu." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrderItem(int orderId, int orderItemId)
    {
        try
        {
            var result = await _orderService.DeleteOrderItemAsync(orderItemId);
            if (!result)
                return Json(new { success = false, message = "Sipariş ürünü bulunamadı veya silinemedi." });
            return Json(new { success = true, message = "Ürün iptal edildi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Sipariş ürünü iptal edilirken hata oluştu. OrderId: {orderId}, OrderItemId: {orderItemId}");
            return Json(new { success = false, message = "Sipariş ürünü iptal edilirken bir hata oluştu." });
        }
    }
}

public class OrderStatusUpdateModel
{
    public string Status { get; set; } = string.Empty;
} 