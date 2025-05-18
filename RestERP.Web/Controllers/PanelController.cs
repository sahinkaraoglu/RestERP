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
    private readonly ITableService _tableService;

    public PanelController(ILogger<PanelController> logger, IFoodService foodService, ITableService tableService)
    {
        _logger = logger;
        _foodService = foodService;
        _tableService = tableService;
    }

    public async Task<IActionResult> Index()
    {
        var menuItemCount = (await _foodService.GetAllFoodsAsync()).Count();
        var foodCategories = FoodCategorySeedData.GetFoodCategories();
        var categoryCount = foodCategories.Count();
        
        var tables = await _tableService.GetAllTablesAsync();
        var totalTables = tables.Count();
        
        var occupiedTables = tables.Count(t => t.IsOccupied == true);
        
        int tableOccupancyPercentage = 0;
        if (totalTables > 0)
        {
            tableOccupancyPercentage = (int)Math.Round((double)occupiedTables / totalTables * 100);
        }
        
        var model = new 
        {
            MenuItemCount = menuItemCount,
            CategoryCount = categoryCount,
            TotalTables = totalTables,
            TableOccupancyPercentage = tableOccupancyPercentage
        };
        return View(model);
    }

    public async Task<IActionResult> Menu()
    {
        try
        {
            var foodcategories = FoodCategorySeedData.GetFoodCategories();
            var foods = await _foodService.GetAllFoodsAsync();
            
            ViewBag.FoodCategories = foodcategories;
            ViewBag.Foods = foods;
            
            return View("Menu/Menu");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Menü sayfası açılırken hata oluştu");
            TempData["ErrorMessage"] = "Menü sayfası açılırken bir hata oluştu: " + ex.Message;
            return View("Error");
        }
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

    public async Task<IActionResult> MenuUpdate(int id)
    {
        try
        {
            var foodcategories = FoodCategorySeedData.GetFoodCategories();
            var food = await _foodService.GetFoodByIdAsync(id);
            
            if (food == null)
            {
                TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                return RedirectToAction("Menu");
            }
            
            ViewBag.FoodCategories = foodcategories;
            ViewBag.Food = food;
            
            return View("Menu/MenuUpdate");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün güncelleme sayfası açılırken hata oluştu");
            TempData["ErrorMessage"] = "Ürün güncelleme sayfası açılırken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Menu");
        }
    }

    [HttpPost]
    public async Task<IActionResult> MenuUpdatePost(int Id, int CategoryId, string Name, string TurkishName, string? Description, decimal Price)
    {
        try
        {
            if (CategoryId <= 0)
            {
                ModelState.AddModelError("CategoryId", "Kategori seçimi zorunludur.");
            }
            
            if (string.IsNullOrEmpty(Name))
            {
                ModelState.AddModelError("Name", "İngilizce ürün adı zorunludur.");
            }
            
            if (string.IsNullOrEmpty(TurkishName))
            {
                ModelState.AddModelError("TurkishName", "Türkçe ürün adı zorunludur.");
            }
            
            if (Price <= 0)
            {
                ModelState.AddModelError("Price", "Fiyat sıfırdan büyük olmalıdır.");
            }

            if (ModelState.ContainsKey("Description"))
            {
                ModelState.Remove("Description");
            }
            
            if (!ModelState.IsValid)
            {
                var foodcategories = FoodCategorySeedData.GetFoodCategories();
                ViewBag.FoodCategories = foodcategories;
                
                var updatedFood = new Food
                {
                    Id = Id,
                    CategoryId = CategoryId,
                    Name = Name,
                    TurkishName = TurkishName,
                    Description = Description,
                    Price = Price
                };
                
                ViewBag.Food = updatedFood;
                return View("Menu/MenuUpdate");
            }
            
            var existingFood = await _foodService.GetFoodByIdAsync(Id);
            if (existingFood == null)
            {
                TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                return RedirectToAction("Menu");
            }
            
            existingFood.CategoryId = CategoryId;
            existingFood.Name = Name;
            existingFood.TurkishName = TurkishName;
            existingFood.Description = Description;
            existingFood.Price = Price;
            
            await _foodService.UpdateFoodAsync(existingFood);
            
            TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
            return RedirectToAction("Menu");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün güncellenirken hata oluştu");
            ModelState.AddModelError("", "Ürün güncellenirken bir hata oluştu: " + ex.Message);
            
            var foodcategories = FoodCategorySeedData.GetFoodCategories();
            ViewBag.FoodCategories = foodcategories;
            
            var updatedFood = new Food
            {
                Id = Id,
                CategoryId = CategoryId,
                Name = Name ?? "",
                TurkishName = TurkishName ?? "",
                Description = Description,
                Price = Price
            };
            
            ViewBag.Food = updatedFood;
            return View("Menu/MenuUpdate");
        }
    }

    public async Task<IActionResult> MenuDelete(int id)
    {
        try
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            
            if (food == null)
            {
                TempData["ErrorMessage"] = "Silinecek ürün bulunamadı.";
                return RedirectToAction("Menu");
            }
            
            ViewBag.Food = food;
            return View("Menu/MenuDelete");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün silme sayfası açılırken hata oluştu");
            TempData["ErrorMessage"] = "Ürün silme sayfası açılırken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Menu");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> MenuDeleteConfirm(int id)
    {
        try
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            
            if (food == null)
            {
                TempData["ErrorMessage"] = "Silinecek ürün bulunamadı.";
                return RedirectToAction("Menu");
            }
            
            await _foodService.DeleteFoodAsync(id);
            
            TempData["SuccessMessage"] = "Ürün başarıyla silindi.";
            return RedirectToAction("Menu");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün silinirken hata oluştu");
            TempData["ErrorMessage"] = "Ürün silinirken bir hata oluştu: " + ex.Message;
            return RedirectToAction("Menu");
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
