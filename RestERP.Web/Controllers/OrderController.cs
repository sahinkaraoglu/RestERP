using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RestERP.Core.Doman.Entities;
using RestERP.Web.Areas.Admin.Models;

namespace RestERP.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IFoodService _foodService;
        private readonly IUserService _userService;
        private readonly ITableService _tableService;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService,
            IFoodService foodService,
            IUserService userService,
            ITableService tableService)
        {
            _logger = logger;
            _orderService = orderService;
            _foodService = foodService;
            _userService = userService;
            _tableService = tableService;
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

                // Tüm siparişleri getir
                var orders = await _orderService.GetAllOrdersAsync();

                // Masa filtresi varsa uygula
                if (tableId.HasValue)
                {
                    orders = orders.Where(o => o.TableId == tableId.Value).ToList();
                    ViewBag.SelectedTableId = tableId.Value;
                }

                // Her bir sipariş için yemek bilgilerini yükle
                foreach (var order in orders)
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
                var orders = await _orderService.GetAllOrdersAsync();
                var tableOrders = orders.Where(o => o.TableId == tableId).ToList();

                foreach (var order in tableOrders)
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
                        TotalPrice = i.Price * i.Quantity
                    }).ToList()
                };

                // Siparişi kaydet
                var result = await _orderService.CreateOrderAsync(order);

                // Başarılı sonuç dön
                return Json(new { success = true, orderId = result.Id, message = "Siparişiniz başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
                return Json(new { success = false, message = "Sipariş oluşturulurken bir hata oluştu: " + ex.Message });
            }
        }
    }
}
