using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Models;
using RestERP.Application.Features.Foods.Queries.GetFoods;
using RestERP.Application.Features.Orders.Queries.GetActiveOrders;
using RestERP.Application.Features.Orders.Queries.GetOrdersByDate;
using RestERP.Application.Features.Orders.Queries.GetOrdersByDateRange;
using RestERP.Application.Features.Tables.Queries.GetTables;
using RestERP.Application.Features.Users.Queries.GetUsers;
using RestERP.Domain.Enums;

namespace RestERP.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class PanelController : Controller
{
    private readonly ILogger<PanelController> _logger;
    private readonly IMediator _mediator;

    public PanelController(
        ILogger<PanelController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var foods = await _mediator.Send(new GetFoodsQuery());
            var menuItemCount = foods.Count();

            var foodCategories = FoodCategorySeedData.GetFoodCategories();
            var categoryCount = foodCategories.Count();

            var tables = await _mediator.Send(new GetTablesQuery());
            var totalTables = tables.Count();

            var activeOrders = (await _mediator.Send(new GetActiveOrdersQuery())).ToList();

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

            var users = (await _mediator.Send(new GetUsersQuery())).ToList();

            var totalEmployees = users.Count(u => u.RoleType == Role.Employee);
            var activeEmployees = users.Count(u => u.RoleType == Role.Employee && u.IsActive);

            var totalCustomers = users.Count(u => u.RoleType == Role.Customer);
            var activeCustomers = users.Count(u => u.RoleType == Role.Customer && u.IsActive);

            var alltotal = totalEmployees + totalCustomers;
            var allactive = activeEmployees + activeCustomers;

            var today = DateTime.Today;
            var todayOrders = (await _mediator.Send(new GetOrdersByDateQuery(today))).ToList();

            var todayOrderCount = todayOrders.Count;
            var todayTotalRevenue = todayOrders.Sum(o => o.TotalAmount);

            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var monthlyOrders = (await _mediator.Send(new GetOrdersByDateRangeQuery(firstDayOfMonth, today))).ToList();

            var monthlyOrderCount = monthlyOrders.Count;
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
            return View("Error", new RestERP.Web.Models.ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
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
