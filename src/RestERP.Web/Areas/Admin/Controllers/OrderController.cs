using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestERP.Core.Domain.Entities;
using RestERP.Web.Areas.Admin.Models;
using System.Text;

namespace RestERP.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public OrderController(
        ILogger<OrderController> logger, 
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "AccessDenied", new { area = "" });
        }
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");

            // Eğer sipariş ID'si belirtilmişse, o siparişin detaylarını göster
            if (Request.Query.ContainsKey("orderId"))
            {
                var orderId = int.TryParse(Request.Query["orderId"], out int id) ? id : (int?)null;
                if (orderId.HasValue)
                {
                    var orderResponse = await httpClient.GetAsync($"api/order/{orderId.Value}/details");
                    if (!orderResponse.IsSuccessStatusCode)
                    {
                        TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                        return RedirectToAction(nameof(Index));
                    }

                    var orderJson = await orderResponse.Content.ReadAsStringAsync();
                    var order = JsonSerializer.Deserialize<Order>(orderJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (order == null)
                    {
                        TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Her bir sipariş kalemi için ürün bilgilerini yükle
                    foreach (var item in order.OrderItems)
                    {
                        var foodResponse = await httpClient.GetAsync($"api/food/{item.FoodId}");
                        if (foodResponse.IsSuccessStatusCode)
                        {
                            var foodJson = await foodResponse.Content.ReadAsStringAsync();
                            item.Food = JsonSerializer.Deserialize<Food>(foodJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
                    }

                    ViewData["ShowSingleOrder"] = true;
                    return View(new List<Order> { order });
                }
            }
            
            // Sipariş ID'si belirtilmemişse tüm aktif siparişleri göster
            var activeOrdersResponse = await httpClient.GetAsync("api/order/active");
            
            if (!activeOrdersResponse.IsSuccessStatusCode)
            {
                ViewData["ShowSingleOrder"] = false;
                return View(new List<Order>());
            }

            var activeOrdersJson = await activeOrdersResponse.Content.ReadAsStringAsync();
            var activeOrders = JsonSerializer.Deserialize<List<Order>>(activeOrdersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            // Eğer aktif sipariş yoksa boş bir liste ile view'ı döndür
            if (activeOrders == null || !activeOrders.Any())
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
                        var foodResponse = await httpClient.GetAsync($"api/food/{item.FoodId}");
                        if (foodResponse.IsSuccessStatusCode)
                        {
                            var foodJson = await foodResponse.Content.ReadAsStringAsync();
                            item.Food = JsonSerializer.Deserialize<Food>(foodJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var ordersResponse = await httpClient.GetAsync($"api/order/table/{tableId}");
            
            if (!ordersResponse.IsSuccessStatusCode)
            {
                TempData["Message"] = "Bu masaya ait aktif sipariş bulunmamaktadır.";
                return RedirectToAction("Index", "Table");
            }

            var ordersJson = await ordersResponse.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(ordersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (orders == null || !orders.Any())
            {
                TempData["Message"] = "Bu masaya ait aktif sipariş bulunmamaktadır.";
                return RedirectToAction("Index", "Table");
            }

            var activeOrder = orders.FirstOrDefault(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled && !o.IsPaid);
            
            if (activeOrder == null)
            {
                TempData["Message"] = "Bu masaya ait aktif sipariş bulunmamaktadır.";
                return RedirectToAction("Index", "Table");
            }

            // Sipariş detaylarını getir
            var orderDetailsResponse = await httpClient.GetAsync($"api/order/{activeOrder.Id}/details");
            
            if (!orderDetailsResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Sipariş detayları alınırken bir hata oluştu.";
                return RedirectToAction("Index", "Table");
            }

            var orderDetailsJson = await orderDetailsResponse.Content.ReadAsStringAsync();
            var orderWithDetails = JsonSerializer.Deserialize<Order>(orderDetailsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (orderWithDetails == null)
            {
                TempData["ErrorMessage"] = "Sipariş detayları alınırken bir hata oluştu.";
                return RedirectToAction("Index", "Table");
            }

            // Her bir sipariş kalemi için ürün bilgilerini yükle
            foreach (var item in orderWithDetails.OrderItems)
            {
                var foodResponse = await httpClient.GetAsync($"api/food/{item.FoodId}");
                if (foodResponse.IsSuccessStatusCode)
                {
                    var foodJson = await foodResponse.Content.ReadAsStringAsync();
                    item.Food = JsonSerializer.Deserialize<Food>(foodJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var activeOrdersResponse = await httpClient.GetAsync("api/order/active");
            
            if (!activeOrdersResponse.IsSuccessStatusCode)
            {
                ViewData["ShowSingleOrder"] = false;
                return View("Index", new List<Order>());
            }

            var activeOrdersJson = await activeOrdersResponse.Content.ReadAsStringAsync();
            var activeOrders = JsonSerializer.Deserialize<List<Order>>(activeOrdersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            // Eğer aktif sipariş yoksa boş bir liste ile view'ı döndür
            if (activeOrders == null || !activeOrders.Any())
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
                        var foodResponse = await httpClient.GetAsync($"api/food/{item.FoodId}");
                        if (foodResponse.IsSuccessStatusCode)
                        {
                            var foodJson = await foodResponse.Content.ReadAsStringAsync();
                            item.Food = JsonSerializer.Deserialize<Food>(foodJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
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

            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var json = JsonSerializer.Serialize(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/order", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Order>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Json(new { success = true, orderId = result?.Id ?? 0 });
            }
            else
            {
                return Json(new { success = false, message = "Sipariş oluşturulurken bir hata oluştu" });
            }
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync($"api/order/table/{tableId}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return Content(json, "application/json");
            }
            
            return StatusCode((int)response.StatusCode, "Siparişler alınırken bir hata oluştu");
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            
            // Status string'ini enum'a çevir
            if (Enum.TryParse(model.Status, out OrderStatus newStatus))
            {
                var json = JsonSerializer.Serialize(newStatus);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"api/order/{orderId}/status", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("Sipariş bulunamadı");
                }
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var ordersResponse = await httpClient.GetAsync("api/order");
            
            if (!ordersResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)ordersResponse.StatusCode, "Siparişler alınırken bir hata oluştu");
            }

            var ordersJson = await ordersResponse.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(ordersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (orders == null)
            {
                return Ok(new List<object>());
            }

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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            
            // Masaya ait aktif siparişleri bul
            var ordersResponse = await httpClient.GetAsync($"api/order/table/{tableId}");
            
            if (!ordersResponse.IsSuccessStatusCode)
            {
                return BadRequest(new { success = false, message = "Bu masada kapatılacak aktif bir hesap yok." });
            }

            var ordersJson = await ordersResponse.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(ordersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (orders == null || !orders.Any())
            {
                return BadRequest(new { success = false, message = "Bu masada kapatılacak aktif bir hesap yok." });
            }

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
                
                var json = JsonSerializer.Serialize(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await httpClient.PutAsync($"api/order/{order.Id}", content);
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            
            var categoriesResponse = await httpClient.GetAsync("api/food/categories");
            var foodsResponse = await httpClient.GetAsync("api/food");
            var imagesResponse = await httpClient.GetAsync("api/food/images");

            if (categoriesResponse.IsSuccessStatusCode && foodsResponse.IsSuccessStatusCode && imagesResponse.IsSuccessStatusCode)
            {
                var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
                var foodsJson = await foodsResponse.Content.ReadAsStringAsync();
                var imagesJson = await imagesResponse.Content.ReadAsStringAsync();

                var categories = JsonSerializer.Deserialize<List<FoodCategory>>(categoriesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var foods = JsonSerializer.Deserialize<List<Food>>(foodsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var images = JsonSerializer.Deserialize<List<Image>>(imagesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                ViewBag.Categories = categories;
                ViewBag.Foods = foods;
                ViewBag.Images = images;
            }

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
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync($"api/order/{id}/details");
            
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Sipariş bulunamadı.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var order = JsonSerializer.Deserialize<Order>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
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
    public async Task<IActionResult> Cancel(int id, bool cancelAll = false, int? tableNumber = null)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var getResponse = await httpClient.GetAsync($"api/order/{id}/details");
            
            if (!getResponse.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Sipariş bulunamadı." });
            }

            var json = await getResponse.Content.ReadAsStringAsync();
            var order = JsonSerializer.Deserialize<Order>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
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
            
            var updateJson = JsonSerializer.Serialize(order);
            var content = new StringContent(updateJson, Encoding.UTF8, "application/json");
            var updateResponse = await httpClient.PutAsync($"api/order/{id}", content);

            if (updateResponse.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Sipariş iptal edilirken bir hata oluştu." });
            }
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var ordersResponse = await httpClient.GetAsync($"api/order/table/{tableNumber}");
            
            if (!ordersResponse.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Siparişler bulunamadı." });
            }

            var ordersJson = await ordersResponse.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(ordersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (orders == null)
            {
                return Json(new { success = false, message = "Siparişler bulunamadı." });
            }

            var activeOrders = orders.Where(o => o.Status != OrderStatus.Cancelled);

            foreach (var order in activeOrders)
            {
                var json = JsonSerializer.Serialize(OrderStatus.Cancelled);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await httpClient.PutAsync($"api/order/{order.Id}/status", content);
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.DeleteAsync($"api/order/item/{orderItemId}");
            
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Ürün iptal edildi." });
            }
            else
            {
                return Json(new { success = false, message = "Sipariş ürünü bulunamadı veya silinemedi." });
            }
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