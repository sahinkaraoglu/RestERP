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
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Kullanıcı girişi kontrolü
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return View(new List<Order>());
                }

                // Kullanıcının siparişlerini getir
                var orders = await _orderService.GetAllOrdersAsync();
                var userOrders = orders.Where(o => o.CustomerId == currentUser.Id).ToList();

                // Her bir sipariş için yemek bilgilerini yükle
                foreach (var order in userOrders)
                {
                    foreach (var item in order.OrderItems)
                    {
                        try
                        {
                            item.Food = await _foodService.GetFoodByIdAsync(item.ProductId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Yemek bilgisi yüklenirken hata oluştu. ProductId: {item.ProductId}");
                        }
                    }
                }

                return View(userOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş sayfası yüklenirken hata oluştu");
                TempData["ErrorMessage"] = "Sipariş yüklenirken bir hata oluştu.";
                return View(new List<Order>());
            }
        }


    }

}
