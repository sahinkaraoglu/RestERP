using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Models;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;

namespace RestERP.Web.Controllers;

public class PanelController : Controller
{
    private readonly ILogger<PanelController> _logger;
    private readonly IFoodService _foodService;

    public PanelController(ILogger<PanelController> logger, IFoodService foodService)
    {
        _logger = logger;
        _foodService = foodService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Menu()
    {
        var foodcategories = FoodCategorySeedData.GetFoodCategories();
        
        var foods = await _foodService.GetAllFoodsAsync();
        
        ViewBag.FoodCategories = foodcategories;
        ViewBag.Foods = foods;
        
        return View("Menu/Menu");
    }

    public IActionResult MenuAdd()
    {
        var categories = FoodCategorySeedData.GetFoodCategories();
        ViewBag.Categories = categories;
        
        return View("Menu/MenuAdd");
    }

    [HttpPost]
    public async Task<IActionResult> AddMenuItem([FromBody] MenuItemViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Model geçersiz: " + string.Join(", ", errors) });
            }
            
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.TurkishName))
            {
                return Json(new { success = false, message = "Ürün adı ve Türkçe adı boş olamaz" });
            }
            
            var food = new Food
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                TurkishName = model.TurkishName,
                Description = model.Description ?? "",
                Price = model.Price,
            };

            await _foodService.CreateFoodAsync(food);

            return Json(new { success = true, message = "Ürün başarıyla eklendi" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün eklenirken hata oluştu");
            return Json(new { success = false, message = "Ürün eklenirken bir hata oluştu: " + ex.Message });
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
