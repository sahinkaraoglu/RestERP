using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestERP.Domain.Entities;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Models;
using RestERP.Web.Services;

namespace RestERP.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FoodCacheService _foodCacheService;

    public HomeController(ILogger<HomeController> logger, FoodCacheService foodCacheService)
    {
        _logger = logger;
        _foodCacheService = foodCacheService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Menu()
    {
        var categories = _foodCacheService.GetCategories();
        var foods = _foodCacheService.GetFoods();
        var images = _foodCacheService.GetImages();
        
        ViewBag.Categories = categories;
        ViewBag.Foods = foods;
        ViewBag.Images = images;
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
