using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using System.Text.Json;

namespace RestERP.Web.Areas.Admin.Panel.Controllers;

public class ReportController : Controller
{
    private readonly ILogger<ReportController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public ReportController(
        ILogger<ReportController> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync("api/order");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Rapor sayfası için siparişler alınırken hata oluştu");
                return View(new List<Order>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(orders ?? new List<Order>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Rapor sayfası yüklenirken hata oluştu");
            return View("Error");
        }
    }
} 