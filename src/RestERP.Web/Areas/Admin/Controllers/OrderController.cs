using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.FoodCategories.Queries.GetFoodCategories;
using RestERP.Application.Features.Foods.Queries.GetFoodImages;
using RestERP.Application.Features.Foods.Queries.GetFoods;
using RestERP.Application.Features.Orders.Commands.CreateOrder;
using RestERP.Application.Features.Orders.Commands.DeleteOrderItem;
using RestERP.Application.Features.Orders.Commands.UpdateOrder;
using RestERP.Application.Features.Orders.Commands.UpdateOrderStatus;
using RestERP.Application.Features.Orders.Queries.GetActiveOrders;
using RestERP.Application.Features.Orders.Queries.GetOrders;
using RestERP.Application.Features.Orders.Queries.GetOrdersByTableId;
using RestERP.Application.Features.Orders.Queries.GetOrderWithDetails;
using RestERP.Application.Features.Users.Queries.GetUserByEmail;
using RestERP.Application.Features.Users.Queries.GetUserById;
using RestERP.Application.Features.Users.Queries.GetUserByUsername;
using RestERP.Domain.Enums;
using RestERP.Core.Domain.Entities;
using RestERP.Web.Areas.Admin.Models;

namespace RestERP.Web.Areas.Admin.Controllers;

[Area("Admin")]
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

    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToAction("Index", "AccessDenied", new { area = "" });
        }

        try
        {
            if (Request.Query.ContainsKey("orderId") &&
                int.TryParse(Request.Query["orderId"], out var orderId))
            {
                var order = await _mediator.Send(new GetOrderWithDetailsQuery(orderId));
                ViewData["ShowSingleOrder"] = true;
                return View(new List<Order> { order });
            }

            var activeOrders = (await _mediator.Send(new GetActiveOrdersQuery())).ToList();
            ViewData["ShowSingleOrder"] = false;
            return View(activeOrders);
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Sipariş bulunamadı.";
            return RedirectToAction(nameof(Index));
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
        try
        {
            var orders = (await _mediator.Send(new GetOrdersByTableIdQuery(tableId))).ToList();
            var activeOrder = orders.FirstOrDefault(o =>
                o.Status != OrderStatus.Completed &&
                o.Status != OrderStatus.Cancelled &&
                !o.IsPaid);

            if (activeOrder == null)
            {
                TempData["Message"] = "Bu masaya ait aktif sipariş bulunmamaktadır.";
                return RedirectToAction("Index", "Table");
            }

            var orderWithDetails = await _mediator.Send(new GetOrderWithDetailsQuery(activeOrder.Id));
            ViewData["ShowSingleOrder"] = true;
            return View("Index", new List<Order> { orderWithDetails });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa {TableId} için siparişler alınırken hata oluştu", tableId);
            TempData["ErrorMessage"] = "Siparişler alınırken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Index", "Table");
        }
    }

    public async Task<IActionResult> ActiveOrders()
    {
        try
        {
            var activeOrders = (await _mediator.Send(new GetActiveOrdersQuery())).ToList();
            ViewData["ShowSingleOrder"] = false;
            return View("Index", activeOrders);
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

            if (model.Items == null || model.Items.Count == 0)
            {
                return Json(new { success = false, message = "Sepette ürün yok." });
            }

            var currentUser = await ResolveCurrentUserAsync();

            var order = new Order
            {
                TableId = model.CustomerInfo.Type == "dine-in" ? model.CustomerInfo.TableNumber : model.TableNumber,
                CustomerId = currentUser?.Id,
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
            var orders = (await _mediator.Send(new GetOrdersByTableIdQuery(tableId))).ToList();
            var result = orders.Select(o => new
            {
                o.Id,
                status = o.Status.ToString(),
                isPaid = o.IsPaid
            });

            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa {TableId} için siparişler alınırken hata oluştu", tableId);
            return StatusCode(500, "Siparişler alınırken bir hata oluştu");
        }
    }

    [HttpPut]
    [Route("api/orders/{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateModel model)
    {
        try
        {
            if (Enum.TryParse(model.Status, out OrderStatus newStatus))
            {
                var updated = await _mediator.Send(new UpdateOrderStatusCommand(orderId, newStatus));
                if (updated)
                {
                    return Ok();
                }

                return NotFound("Sipariş bulunamadı");
            }

            return BadRequest("Geçersiz sipariş durumu");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş durumu güncellenirken hata oluştu. OrderId: {OrderId}", orderId);
            return StatusCode(500, "Sipariş durumu güncellenirken bir hata oluştu");
        }
    }

    [HttpGet]
    [Route("api/orders/all-tables")]
    public async Task<IActionResult> GetAllTablesOrders()
    {
        try
        {
            var orders = (await _mediator.Send(new GetOrdersQuery())).ToList();
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
            var orders = (await _mediator.Send(new GetOrdersByTableIdQuery(tableId))).ToList();
            var activeOrders = orders
                .Where(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled && !o.IsPaid)
                .ToList();

            if (!activeOrders.Any())
            {
                return BadRequest(new { success = false, message = "Bu masada kapatılacak aktif bir hesap yok." });
            }

            foreach (var summary in activeOrders)
            {
                var order = await _mediator.Send(new GetOrderWithDetailsQuery(summary.Id));
                order.IsPaid = true;
                order.Status = OrderStatus.Completed;

                foreach (var item in order.OrderItems)
                {
                    item.IsPaid = true;
                }

                await _mediator.Send(new UpdateOrderCommand(order));
            }

            return Ok(new { success = true, message = "Hesap kapatıldı" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa {TableId} için hesap kapatma işlemi sırasında hata oluştu", tableId);
            return StatusCode(500, new { success = false, message = "Hesap kapatılırken bir hata oluştu." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create(int tableId)
    {
        try
        {
            ViewBag.Categories = (await _mediator.Send(new GetFoodCategoriesQuery())).ToList();
            ViewBag.Foods = (await _mediator.Send(new GetFoodsQuery())).ToList();
            ViewBag.Images = (await _mediator.Send(new GetFoodImagesQuery())).ToList();
            ViewBag.TableId = tableId;

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş oluşturma sayfası açılırken hata oluştu");
            TempData["ErrorMessage"] = "Sipariş oluşturma sayfası açılırken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Index", "Table");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id, bool cancelAll = false, int? tableNumber = null)
    {
        try
        {
            var order = await _mediator.Send(new GetOrderWithDetailsQuery(id));

            foreach (var item in order.OrderItems)
            {
                item.Status = OrderStatus.Cancelled;
            }

            order.Status = OrderStatus.Cancelled;
            await _mediator.Send(new UpdateOrderCommand(order));

            return Json(new { success = true });
        }
        catch (KeyNotFoundException)
        {
            return Json(new { success = false, message = "Sipariş bulunamadı." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş iptal edilirken hata oluştu. OrderId: {OrderId}", id);
            return Json(new { success = false, message = "Sipariş iptal edilirken bir hata oluştu." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CancelTableOrders(int tableNumber)
    {
        try
        {
            var orders = (await _mediator.Send(new GetOrdersByTableIdQuery(tableNumber))).ToList();
            var activeOrders = orders.Where(o => o.Status != OrderStatus.Cancelled);

            foreach (var order in activeOrders)
            {
                await _mediator.Send(new UpdateOrderStatusCommand(order.Id, OrderStatus.Cancelled));
            }

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa {TableNumber} siparişleri iptal edilirken hata oluştu", tableNumber);
            return Json(new { success = false, message = "Siparişler iptal edilirken bir hata oluştu." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrderItem(int orderId, int orderItemId)
    {
        try
        {
            var deleted = await _mediator.Send(new DeleteOrderItemCommand(orderItemId));
            if (deleted)
            {
                return Json(new { success = true, message = "Ürün iptal edildi." });
            }

            return Json(new { success = false, message = "Sipariş ürünü bulunamadı veya silinemedi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sipariş ürünü iptal edilirken hata oluştu. OrderId: {OrderId}, OrderItemId: {OrderItemId}", orderId, orderItemId);
            return Json(new { success = false, message = "Sipariş ürünü iptal edilirken bir hata oluştu." });
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

public class OrderStatusUpdateModel
{
    public string Status { get; set; } = string.Empty;
}
