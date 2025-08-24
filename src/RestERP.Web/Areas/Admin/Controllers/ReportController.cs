using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;

namespace RestERP.Web.Areas.Admin.Panel.Controllers;

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
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Rapor sayfası yüklenirken hata oluştu");
            return View("Error");
        }
    }
} 