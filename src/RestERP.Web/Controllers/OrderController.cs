using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using RestERP.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
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
        

        public OrderController(
            ILogger<OrderController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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
                // Tüm masaları API'den getir
                var clientForTables = CreateHttpClient();
                var tablesResponse = await clientForTables.GetAsync("api/table");
                if (tablesResponse.IsSuccessStatusCode)
                {
                    var tablesJson = await tablesResponse.Content.ReadAsStringAsync();
                    var tablesOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var tables = JsonSerializer.Deserialize<List<Table>>(tablesJson, tablesOptions) ?? new List<Table>();
                    ViewBag.Tables = tables;
                }
                else
                {
                    _logger.LogWarning("Masalar yüklenemedi. Status: {StatusCode}", tablesResponse.StatusCode);
                    ViewBag.Tables = new List<Table>();
                }

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
                    var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(new { success = false, message = string.IsNullOrWhiteSpace(errors) ? "Geçersiz sipariş bilgileri." : errors });
                }

                if (User.Identity?.IsAuthenticated != true)
                {
                    return Unauthorized(new { success = false, message = "Sipariş verebilmek için giriş yapmalısınız." });
                }

                var client = CreateHttpClient();
                var currentUser = await ResolveCurrentUserAsync(client);
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
                /* reuse existing client */
                var jsonContent = JsonSerializer.Serialize(order);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("api/order", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API'ye sipariş gönderilemedi. Status: {StatusCode}, Error: {Error}", response.StatusCode, errorContent);
                    return Json(new { success = false, message = "Sipariş oluşturulurken bir hata oluştu." + (string.IsNullOrWhiteSpace(errorContent) ? "" : " " + errorContent) });
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

        private async Task<ApplicationUser?> ResolveCurrentUserAsync(HttpClient client)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            async Task<ApplicationUser?> TryGetAsync(string url)
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApplicationUser>(json, options);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userId, out var id) && id > 0)
            {
                var byId = await TryGetAsync($"api/user/{id}");
                if (byId != null)
                {
                    return byId;
                }
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(email))
            {
                var byEmail = await TryGetAsync($"api/user/email/{Uri.EscapeDataString(email)}");
                if (byEmail != null)
                {
                    return byEmail;
                }
            }

            var name = User.Identity?.Name;
            if (!string.IsNullOrWhiteSpace(name))
            {
                var byUsername = await TryGetAsync($"api/user/username/{Uri.EscapeDataString(name)}");
                if (byUsername != null)
                {
                    return byUsername;
                }

                return await TryGetAsync($"api/user/email/{Uri.EscapeDataString(name)}");
            }

            return null;
        }
    }
}
