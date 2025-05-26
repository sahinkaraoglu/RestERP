using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;

namespace RestERP.Web.Controllers;

public class ReservationController : Controller
{
    private readonly ILogger<ReservationController> _logger;
    private readonly ITableService _tableService;

    public ReservationController(
        ILogger<ReservationController> logger,
        ITableService tableService)
    {
        _logger = logger;
        _tableService = tableService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var tables = await _tableService.GetAllTablesAsync();
            return View(tables);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Rezervasyon sayfası yüklenirken hata oluştu");
            return View("Error");
        }
    }
} 