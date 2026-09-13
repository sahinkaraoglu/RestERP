using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Web.Controllers;

public class ReportController : Controller
{
    private readonly ILogger<ReportController> _logger;
    private readonly IOrderService _orderService;

    public ReportController(
        ILogger<ReportController> logger,
        IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var orders = (await _orderService.GetAllOrdersAsync()).ToList();
            return View(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Rapor sayfası yüklenirken hata oluştu");
            TempData["ErrorMessage"] = "Rapor sayfası yüklenirken bir hata oluştu: " + ex.Message;
            return View(new List<Order>());
        }
    }
}
