using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Models;
using RestERP.Application.Services.Abstract;
using RestERP.Domain.Enums;
using RestERP.Core.Domain.Entities;
using System.Text.Json;

namespace RestERP.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class PanelController : Controller
{
    private readonly ILogger<PanelController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public PanelController(
        ILogger<PanelController> logger, 
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
            
            // Yemek sayısını al
            var foodsResponse = await httpClient.GetAsync("api/food");
            var menuItemCount = 0;
            if (foodsResponse.IsSuccessStatusCode)
            {
                var foodsJson = await foodsResponse.Content.ReadAsStringAsync();
                var foods = JsonSerializer.Deserialize<List<Food>>(foodsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                menuItemCount = foods?.Count ?? 0;
            }
            
            var foodCategories = FoodCategorySeedData.GetFoodCategories();
            var categoryCount = foodCategories.Count();
            
            // Tüm masaları al
            var tablesResponse = await httpClient.GetAsync("api/table");
            var totalTables = 0;
            if (tablesResponse.IsSuccessStatusCode)
            {
                var tablesJson = await tablesResponse.Content.ReadAsStringAsync();
                var tables = JsonSerializer.Deserialize<List<Table>>(tablesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                totalTables = tables?.Count ?? 0;
            }
            
            // Tüm aktif siparişleri al
            var activeOrdersResponse = await httpClient.GetAsync("api/order/active");
            var activeOrders = new List<Order>();
            if (activeOrdersResponse.IsSuccessStatusCode)
            {
                var activeOrdersJson = await activeOrdersResponse.Content.ReadAsStringAsync();
                activeOrders = JsonSerializer.Deserialize<List<Order>>(activeOrdersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Order>();
            }
            
            // Aktif siparişi olan benzersiz masa sayısını hesapla
            var occupiedTables = activeOrders
                .Select(o => o.TableId)
                .Distinct()
                .Count();

            var occupiedOrders = activeOrders
                .Select(o => o.OrderNumber)
                .Distinct()
                .Count();

            int tableOccupancyPercentage = 0;
            if (totalTables > 0)
            {
                tableOccupancyPercentage = (int)Math.Round((double)occupiedTables / totalTables * 100);
            }
            
            // Kullanıcıları çekerek çalışan sayısını hesaplayalım
            var usersResponse = await httpClient.GetAsync("api/user");
            var users = new List<ApplicationUser>();
            if (usersResponse.IsSuccessStatusCode)
            {
                var usersJson = await usersResponse.Content.ReadAsStringAsync();
                users = JsonSerializer.Deserialize<List<ApplicationUser>>(usersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ApplicationUser>();
            }

            var totalEmployees = users.Count(u => u.RoleType == Role.Employee);
            var activeEmployees = users.Count(u => u.RoleType == Role.Employee && u.IsActive);

            var totalCustomers = users.Count(u => u.RoleType == Role.Customer);
            var activeCustomers = users.Count(u => u.RoleType == Role.Customer && u.IsActive);

            var alltotal = totalEmployees + totalCustomers;
            var allactive = activeEmployees + activeCustomers;

            var today = DateTime.Today;
            var todayOrdersResponse = await httpClient.GetAsync($"api/order/date/{today:yyyy-MM-dd}");
            var todayOrders = new List<Order>();
            if (todayOrdersResponse.IsSuccessStatusCode)
            {
                var todayOrdersJson = await todayOrdersResponse.Content.ReadAsStringAsync();
                todayOrders = JsonSerializer.Deserialize<List<Order>>(todayOrdersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Order>();
            }
            
            var todayOrderCount = todayOrders.Count();
            var todayTotalRevenue = todayOrders.Sum(o => o.TotalAmount);

            // Aylık istatistikleri hesapla
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var monthlyOrdersResponse = await httpClient.GetAsync($"api/order/daterange?startDate={firstDayOfMonth:yyyy-MM-dd}&endDate={today:yyyy-MM-dd}");
            var monthlyOrders = new List<Order>();
            if (monthlyOrdersResponse.IsSuccessStatusCode)
            {
                var monthlyOrdersJson = await monthlyOrdersResponse.Content.ReadAsStringAsync();
                monthlyOrders = JsonSerializer.Deserialize<List<Order>>(monthlyOrdersJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Order>();
            }
            
            var monthlyOrderCount = monthlyOrders.Count();
            var monthlyRevenue = monthlyOrders.Sum(o => o.TotalAmount);

            var model = new 
            {
                MenuItemCount = menuItemCount,
                CategoryCount = categoryCount,
                TotalTables = totalTables,
                TableOccupancyPercentage = tableOccupancyPercentage,
                TotalEmployees = totalEmployees,
                ActiveEmployees = activeEmployees,
                OccupiedTables = occupiedTables,
                OccupiedOrders = occupiedOrders,
                AllTotal = alltotal,
                AllActive = allactive,
                TodayOrderCount = todayOrderCount,
                TodayTotalRevenue = todayTotalRevenue,
                MonthlyOrderCount = monthlyOrderCount,
                MonthlyRevenue = monthlyRevenue
            };
            
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Panel sayfası yüklenirken hata oluştu");
            TempData["ErrorMessage"] = "Panel sayfası yüklenirken bir hata oluştu: " + ex.Message;
            return View("Error");
        }
    }


    public IActionResult Panel()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
