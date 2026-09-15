using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.Orders.Commands.CreateOrder;
using RestERP.Application.Features.Orders.Queries.GetActiveOrders;
using RestERP.Application.Features.Orders.Queries.GetOrdersByTableId;
using RestERP.Application.Features.Tables.Queries.GetTables;
using RestERP.Application.Features.Users.Queries.GetUserByEmail;
using RestERP.Application.Features.Users.Queries.GetUserById;
using RestERP.Application.Features.Users.Queries.GetUserByUsername;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Enums;
using RestERP.Web.Areas.Admin.Models;

namespace RestERP.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IMediator _mediator;

        public OrderController(
            ILogger<OrderController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index(int? tableId = null)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Index", "AccessDenied");
            }

            try
            {
                ViewBag.Tables = (await _mediator.Send(new GetTablesQuery())).ToList();

                var orders = (await _mediator.Send(new GetActiveOrdersQuery())).ToList();
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
                ViewBag.Tables ??= new List<Table>();
                return View(new List<Order>());
            }
        }

        public async Task<IActionResult> ViewOrder(int tableId)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Index", "AccessDenied");
            }

            try
            {
                var orders = await _mediator.Send(new GetOrdersByTableIdQuery(tableId));
                var tableOrders = orders
                    .Where(o => !o.IsPaid && o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled)
                    .ToList();

                return View(tableOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa {TableId} siparişleri yüklenirken hata oluştu", tableId);
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

                var currentUser = await ResolveCurrentUserAsync();
                if (currentUser == null)
                {
                    return BadRequest(new { success = false, message = "Kullanıcı bilgileri bulunamadı." });
                }

                if (model.Items == null || model.Items.Count == 0)
                {
                    return BadRequest(new { success = false, message = "Sepette ürün yok." });
                }

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

                var result = await _mediator.Send(new CreateOrderCommand(order));
                return Json(new { success = true, orderId = result.Id, message = "Siparişiniz başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
                return Json(new { success = false, message = "Sipariş oluşturulurken bir hata oluştu." });
            }
        }

        private async Task<ApplicationUser?> ResolveCurrentUserAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userId, out var id) && id > 0)
            {
                var byId = await _mediator.Send(new GetUserByIdQuery(id));
                if (byId != null)
                {
                    return byId;
                }
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(email))
            {
                var byEmail = await _mediator.Send(new GetUserByEmailQuery(email));
                if (byEmail != null)
                {
                    return byEmail;
                }
            }

            var name = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            return await _mediator.Send(new GetUserByUsernameQuery(name))
                ?? await _mediator.Send(new GetUserByEmailQuery(name));
        }
    }
}
