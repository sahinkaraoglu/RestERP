using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Models;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Enums;
using RestERP.Core.Doman.Entities;
using RestERP.Web.Services;

namespace RestERP.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class PanelController : Controller
{
    private readonly ILogger<PanelController> _logger;
    private readonly IFoodService _foodService;
    private readonly ITableService _tableService;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly FoodCacheService _foodCacheService;

    public PanelController(
        ILogger<PanelController> logger, 
        IFoodService foodService, 
        ITableService tableService,
        IUserService userService,
        IOrderService orderService,
        FoodCacheService foodCacheService)
    {
        _logger = logger;
        _foodService = foodService;
        _tableService = tableService;
        _userService = userService;
        _orderService = orderService;
        _foodCacheService = foodCacheService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var menuItemCount = (await _foodService.GetAllFoodsAsync()).Count();
            var foodCategories = FoodCategorySeedData.GetFoodCategories();
            var categoryCount = foodCategories.Count();
            
            // Tüm masaları al
            var tables = await _tableService.GetAllTablesAsync();
            var totalTables = tables.Count();
            
            // Tüm aktif siparişleri al
            var activeOrders = await _orderService.GetActiveOrdersAsync();
            
            // Aktif siparişi olan benzersiz masa sayısını hesapla
            var occupiedTables = activeOrders
                .Select(o => o.TableId)
                .Distinct()
                .Count();
            
            int tableOccupancyPercentage = 0;
            if (totalTables > 0)
            {
                tableOccupancyPercentage = (int)Math.Round((double)occupiedTables / totalTables * 100);
            }
            
            // Kullanıcıları çekerek çalışan sayısını hesaplayalım
            var users = await _userService.GetAllUsersAsync();

            var totalEmployees = users.Count(u => u.RoleType == Role.Employee);
            var activeEmployees = users.Count(u => u.RoleType == Role.Employee && u.IsActive);

            var totalCustomers = users.Count(u => u.RoleType == Role.Customer);
            var activeCustomers = users.Count(u => u.RoleType == Role.Customer && u.IsActive);

            var alltotal = totalEmployees + activeEmployees;
            var allactive = activeEmployees + activeCustomers;

            var model = new 
            {
                MenuItemCount = menuItemCount,
                CategoryCount = categoryCount,
                TotalTables = totalTables,
                TableOccupancyPercentage = tableOccupancyPercentage,
                TotalEmployees = totalEmployees,
                ActiveEmployees = activeEmployees,
                OccupiedTables = occupiedTables,
                AllTotal = alltotal,
                AllActive = allactive

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
