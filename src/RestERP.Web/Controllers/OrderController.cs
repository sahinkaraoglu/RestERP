using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RestERP.Core.Domain.Entities;
using RestERP.Web.Areas.Admin.Models;
using System.Text.Json;
using System.Net.Http.Headers;

namespace RestERP.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;
        private readonly ITableService _tableService;

        public OrderController(
            ILogger<OrderController> logger,
            IHttpClientFactory httpClientFactory,
            IUserService userService,
            ITableService tableService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _userService = userService;
            _tableService = tableService;
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient("RestERPApi");
            
            // JWT token'ı cookie'den al ve header'a ekle
            var token = Request.Cookies["JWT"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            return client;
        }

        public async Task<IActionResult> Index(int? tableId = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccessDenied");
            }

            try
            {
                // Tüm masaları getir
                var tables = await _tableService.GetAllTablesAsync();
                ViewBag.Tables = tables;

                // API'den aktif siparişleri getir
                var client = CreateHttpClient();
                var response = await client.GetAsync("api/order/active");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("API'den sipariş verileri alınamadı. Status: {StatusCode}", response.StatusCode);
                    TempData["ErrorMessage"] = "Sipariş verileri yüklenirken bir hata oluştu.";
                    return View(new List<Order>());
                }

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var orders = JsonSerializer.Deserialize<List<Order>>(json, options) ?? new List<Order>();

                // Masa filtresi varsa uygula
                if (tableId.HasValue)
                {
                    orders = orders.Where(o => o.TableId == tableId.Value).ToList();
                    ViewBag.SelectedTableId = tableId.Value;
                }

                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş sayfası yüklenirken hata oluştu");
                TempData["ErrorMessage"] = "Sipariş yüklenirken bir hata oluştu.";
                return View(new List<Order>());
            }
        }

        public async Task<IActionResult> ViewOrder(int tableId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccessDenied");
            }

            try
            {
                // API'den masaya göre siparişleri getir
                var client = CreateHttpClient();
                var response = await client.GetAsync($"api/order/table/{tableId}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("API'den masa siparişleri alınamadı. TableId: {TableId}, Status: {StatusCode}", tableId, response.StatusCode);
                    TempData["ErrorMessage"] = "Siparişler yüklenirken bir hata oluştu.";
                    return View(new List<Order>());
                }

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var orders = JsonSerializer.Deserialize<List<Order>>(json, options) ?? new List<Order>();

                // Filtrele: ödenmemiş ve aktif siparişler
                var tableOrders = orders.Where(o => !o.IsPaid && o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled).ToList();

                return View(tableOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Masa {tableId} siparişleri yüklenirken hata oluştu");
                TempData["ErrorMessage"] = "Siparişler yüklenirken bir hata oluştu.";
                return View(new List<Order>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Geçersiz sipariş bilgileri." });
                }

                // Kullanıcı kontrolü
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { success = false, message = "Sipariş verebilmek için giriş yapmalısınız." });
                }

                var currentUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);
                if (currentUser == null)
                {
                    return BadRequest(new { success = false, message = "Kullanıcı bilgileri bulunamadı." });
                }

                // Sipariş oluştur
                var order = new Order
                {
                    TableId = model.CustomerInfo.Type == "dine-in" ? model.CustomerInfo.TableNumber : null,
                    CustomerId = currentUser.Id,
                    Status = OrderStatus.New,
                    TotalAmount = model.Items.Sum(i => i.Price * i.Quantity),
                    OrderItems = model.Items.Select(i => new OrderItem
                    {
                        FoodId = i.FoodId,
                        Quantity = i.Quantity,
                        UnitPrice = i.Price,
                        Status = OrderStatus.New,
                        TotalPrice = i.Price * i.Quantity
                    }).ToList()
                };

                // API'ye sipariş gönder
                var client = CreateHttpClient();
                var jsonContent = JsonSerializer.Serialize(order);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("api/order", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API'ye sipariş gönderilemedi. Status: {StatusCode}, Error: {Error}", response.StatusCode, errorContent);
                    return Json(new { success = false, message = "Sipariş oluşturulurken bir hata oluştu." });
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<Order>(responseJson, options);

                // Başarılı sonuç dön
                return Json(new { success = true, orderId = result?.Id, message = "Siparişiniz başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
                
                // İç hata detaylarını da logla
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    _logger.LogError(innerException, "İç hata: {Message}", innerException.Message);
                    innerException = innerException.InnerException;
                }
                
                return Json(new { success = false, message = "Sipariş oluşturulurken bir hata oluştu: " + ex.Message + (ex.InnerException != null ? " | İç hata: " + ex.InnerException.Message : "") });
            }
        }
    }
}
