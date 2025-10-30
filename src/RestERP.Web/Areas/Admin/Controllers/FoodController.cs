using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Areas.Admin.Models;
using RestERP.Web.Services;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodController : Controller
    {
        private readonly ILogger<FoodController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FoodCacheService _foodCacheService;
        private readonly IWebHostEnvironment _env;

        public FoodController(
            ILogger<FoodController> logger,
            IHttpClientFactory httpClientFactory,
            FoodCacheService foodCacheService,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _foodCacheService = foodCacheService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = _foodCacheService.GetCategories();
                var foods = _foodCacheService.GetFoods();

                ViewBag.FoodCategories = categories;
                ViewBag.Foods = foods;

                return View("~/Areas/Admin/Views/Food/Index.cshtml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Menü sayfası açılırken hata oluştu");
                TempData["ErrorMessage"] = "Menü sayfası açılırken bir hata oluştu: " + ex.Message;
                return View("Error");
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

                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var json = JsonSerializer.Serialize(food);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/food", content);

                if (response.IsSuccessStatusCode)
                {
                    _foodCacheService.ClearCache();
                    return Json(new { success = true, message = "Ürün başarıyla eklendi" });
                }
                else
                {
                    return Json(new { success = false, message = "Ürün eklenirken bir hata oluştu" });
                }
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
                
                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var response = await httpClient.GetAsync($"api/food/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                    return RedirectToAction("Index", "Food", new { area = "Admin" });
                }

                var json = await response.Content.ReadAsStringAsync();
                var food = JsonSerializer.Deserialize<Food>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (food == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                    return RedirectToAction("Index", "Food", new { area = "Admin" });
                }

                ViewBag.FoodCategories = foodcategories;
                ViewBag.Food = food;
                ViewBag.Images = _foodCacheService.GetImages();

                return View("~/Areas/Admin/Views/Food/Edit.cshtml");
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
                    ViewBag.Images = _foodCacheService.GetImages();

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
                    return View("~/Areas/Admin/Views/Food/Edit.cshtml");
                }

                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var getResponse = await httpClient.GetAsync($"api/food/{Id}");
                
                if (!getResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                    return RedirectToAction("Index", "Food", new { area = "Admin" });
                }

                var existingJson = await getResponse.Content.ReadAsStringAsync();
                var existingFood = JsonSerializer.Deserialize<Food>(existingJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (existingFood == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek ürün bulunamadı.";
                    return RedirectToAction("Index", "Food", new { area = "Admin" });
                }

                existingFood.CategoryId = CategoryId;
                existingFood.Name = Name;
                existingFood.TurkishName = TurkishName;
                existingFood.Description = Description;
                existingFood.Price = Price;

                // Görsel yükleme işlemi (opsiyonel)
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

                    // Web köküne göre göreli yol
                    var relativePath = $"/img/Food/Uploads/{fileName}";

                    // Var olan ilk görseli güncelle, yoksa yeni ekle
                    if (existingFood.Images != null && existingFood.Images.Any())
                    {
                        var firstImage = existingFood.Images.First();
                        firstImage.Path = relativePath;
                        firstImage.FoodId = existingFood.Id;
                    }
                    else
                    {
                        existingFood.Images = new List<Image>
                        {
                            new Image { Path = relativePath, FoodId = existingFood.Id }
                        };
                    }
                }

                var json = JsonSerializer.Serialize(existingFood);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var updateResponse = await httpClient.PutAsync($"api/food/{Id}", content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    _foodCacheService.ClearCache();
                    TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
                    return RedirectToAction("Index", "Food", new { area = "Admin" });
                }
                else
                {
                    TempData["ErrorMessage"] = "Ürün güncellenirken bir hata oluştu.";
                    return RedirectToAction("Index", "Food", new { area = "Admin" });
                }
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
                return View("~/Areas/Admin/Views/Food/Edit.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var response = await httpClient.DeleteAsync($"api/food/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    _foodCacheService.ClearCache();
                    return Json(new { success = true, message = "Ürün başarıyla silindi." });
                }
                else
                {
                    return Json(new { success = false, message = "Silinecek ürün bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün silinirken hata oluştu");
                return Json(new { success = false, message = "Ürün silinirken bir hata oluştu: " + ex.Message });
            }
        }
    }
}


