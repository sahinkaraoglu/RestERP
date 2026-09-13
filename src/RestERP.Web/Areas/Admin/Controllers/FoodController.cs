using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Areas.Admin.Models;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodController : Controller
    {
        private readonly ILogger<FoodController> _logger;
        private readonly IFoodService _foodService;
        private readonly IWebHostEnvironment _env;

        public FoodController(
            ILogger<FoodController> logger,
            IFoodService foodService,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _foodService = foodService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = (await _foodService.GetAllFoodCategoriesAsync()).ToList();
                var foods = (await _foodService.GetAllFoodsAsync()).ToList();

                ViewBag.FoodCategories = categories;
                ViewBag.Foods = foods;

                return View("~/Areas/Admin/Views/Food/Index.cshtml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Menü sayfası açılırken hata oluştu");
                TempData["ErrorMessage"] = "Menü sayfası açılırken bir hata oluştu: " + ex.Message;
                return View("Error", new RestERP.Web.Models.ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Create()
        {
            var categories = FoodCategorySeedData.GetFoodCategories();
            ViewBag.Categories = categories;

            return View("~/Areas/Admin/Views/Food/Create.cshtml");
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
                return Json(new { success = true, message = "Ürün başarıyla eklendi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün eklenirken hata oluştu");
                return Json(new { success = false, message = "Ürün eklenirken bir hata oluştu: " + ex.Message });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var foodcategories = FoodCategorySeedData.GetFoodCategories();
                var food = await _foodService.GetFoodByIdAsync(id);
                var images = (await _foodService.GetAllFoodImagesAsync())
                    .Where(i => i.FoodId == id)
                    .ToList();

                ViewBag.FoodCategories = foodcategories;
                ViewBag.Food = food;
                ViewBag.Images = images;

                return View("~/Areas/Admin/Views/Food/Edit.cshtml");
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                return RedirectToAction("Index", "Food", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün güncelleme sayfası açılırken hata oluştu");
                TempData["ErrorMessage"] = "Ürün güncelleme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index", "Food", new { area = "Admin" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, int CategoryId, string Name, string TurkishName, string? Description, decimal Price, IFormFile? ImageFile)
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
                    ViewBag.Images = (await _foodService.GetAllFoodImagesAsync())
                        .Where(i => i.FoodId == Id)
                        .ToList();

                    ViewBag.Food = new Food
                    {
                        Id = Id,
                        CategoryId = CategoryId,
                        Name = Name,
                        TurkishName = TurkishName,
                        Description = Description,
                        Price = Price
                    };

                    return View("~/Areas/Admin/Views/Food/Edit.cshtml");
                }

                var existingFood = await _foodService.GetFoodByIdAsync(Id);

                existingFood.CategoryId = CategoryId;
                existingFood.Name = Name;
                existingFood.TurkishName = TurkishName;
                existingFood.Description = Description;
                existingFood.Price = Price;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsRoot = Path.Combine(_env.WebRootPath ?? string.Empty, "img", "Food", "Uploads");
                    if (!Directory.Exists(uploadsRoot))
                    {
                        Directory.CreateDirectory(uploadsRoot);
                    }

                    var extension = Path.GetExtension(ImageFile.FileName);
                    var fileName = $"food_{Id}_{DateTime.UtcNow:yyyyMMddHHmmssfff}{extension}";
                    var physicalPath = Path.Combine(uploadsRoot, fileName);

                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    var relativePath = $"/img/Food/Uploads/{fileName}";
                    await _foodService.SaveFoodImageAsync(Id, relativePath);
                }

                await _foodService.UpdateFoodAsync(existingFood);

                TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
                return RedirectToAction("Index", "Food", new { area = "Admin" });
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                return RedirectToAction("Index", "Food", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün güncellenirken hata oluştu");
                ModelState.AddModelError("", "Ürün güncellenirken bir hata oluştu: " + ex.Message);

                var foodcategories = FoodCategorySeedData.GetFoodCategories();
                ViewBag.FoodCategories = foodcategories;
                ViewBag.Food = new Food
                {
                    Id = Id,
                    CategoryId = CategoryId,
                    Name = Name ?? "",
                    TurkishName = TurkishName ?? "",
                    Description = Description,
                    Price = Price
                };

                return View("~/Areas/Admin/Views/Food/Edit.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _foodService.DeleteFoodAsync(id);
                return Json(new { success = true, message = "Ürün başarıyla silindi." });
            }
            catch (KeyNotFoundException)
            {
                return Json(new { success = false, message = "Silinecek ürün bulunamadı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün silinirken hata oluştu");
                return Json(new { success = false, message = "Ürün silinirken bir hata oluştu: " + ex.Message });
            }
        }
    }
}
