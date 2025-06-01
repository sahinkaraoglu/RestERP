using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;
using RestERP.Core.Domain.Entities;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Areas.Admin.Models;
using RestERP.Web.Services;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IFoodService _foodService;
        private readonly ITableService _tableService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly FoodCacheService _foodCacheService;

        public MenuController(
            ILogger<MenuController> logger,
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
                var categories = _foodCacheService.GetCategories();
                var foods = _foodCacheService.GetFoods();

                ViewBag.FoodCategories = categories;
                ViewBag.Foods = foods;

                return View();
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

            return View("~/Areas/Admin/Views/Menu/MenuAdd.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuViewModel model)
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
                _foodCacheService.ClearCache();

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
                    return RedirectToAction("Index", "Menu", new { area = "Admin" });
                }

                ViewBag.FoodCategories = foodcategories;
                ViewBag.Food = food;

                return View("~/Areas/Admin/Views/Menu/MenuUpdate.cshtml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün güncelleme sayfası açılırken hata oluştu");
                TempData["ErrorMessage"] = "Ürün güncelleme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index", "Menu", new { area = "Admin" });
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
                    return View("~/Areas/Admin/Views/Menu/MenuUpdate.cshtml");
                }

                var existingFood = await _foodService.GetFoodByIdAsync(Id);
                if (existingFood == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                    return RedirectToAction("Index", "Menu", new { area = "Admin" });
                }

                existingFood.CategoryId = CategoryId;
                existingFood.Name = Name;
                existingFood.TurkishName = TurkishName;
                existingFood.Description = Description;
                existingFood.Price = Price;

                await _foodService.UpdateFoodAsync(existingFood);
                _foodCacheService.ClearCache();

                TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
                return RedirectToAction("Index", "Menu", new { area = "Admin" });
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
                return View("~/Areas/Admin/Views/Menu/MenuUpdate.cshtml");
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
                    return RedirectToAction("Index", "Menu", new { area = "Admin" });
                }
                
                ViewBag.Food = food;
                return View("~/Areas/Admin/Views/Menu/MenuDelete.cshtml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün silme sayfası açılırken hata oluştu");
                TempData["ErrorMessage"] = "Ürün silme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index", "Menu", new { area = "Admin" });
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
                    return RedirectToAction("Index", "Menu", new { area = "Admin" });
                }
                
                await _foodService.DeleteFoodAsync(id);
                _foodCacheService.ClearCache();
                
                TempData["SuccessMessage"] = "Ürün başarıyla silindi.";
                return RedirectToAction("Index", "Menu", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün silinirken hata oluştu");
                TempData["ErrorMessage"] = "Ürün silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index", "Menu", new { area = "Admin" });
            }
        }
    }
} 