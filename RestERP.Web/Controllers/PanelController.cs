using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Models;

namespace RestERP.Web.Controllers;

public class PanelController : Controller
{
    private readonly ILogger<PanelController> _logger;

    public PanelController(ILogger<PanelController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Menu()
    {
        var categories = FoodCategorySeedData.GetFoodCategories();
        var subCategories = FoodSeedData.GetFood();
        
        ViewBag.Categories = categories;
        ViewBag.SubCategories = subCategories;
        
        return View("Menu/Menu");
    }

    public IActionResult MenuAdd()
    {
        var categories = FoodCategorySeedData.GetFoodCategories();
        ViewBag.Categories = categories;
        
        return View("Menu/MenuAdd");
    }

    [HttpPost]
    public IActionResult AddMenuItem(MenuItemViewModel model)
    {
        return Json(new { success = true, message = "Ürün başarıyla eklendi" });
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
