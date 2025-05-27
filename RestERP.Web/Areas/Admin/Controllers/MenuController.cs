using Microsoft.AspNetCore.Mvc;
using RestERP.Web.Services;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly FoodCacheService _foodCacheService;

        public MenuController(FoodCacheService foodCacheService)
        {
            _foodCacheService = foodCacheService;
        }

        public IActionResult Index()
        {
            var categories = _foodCacheService.GetCategories();
            var foods = _foodCacheService.GetFoods();
            
            ViewBag.FoodCategories = categories;
            ViewBag.Foods = foods;
            
            return View();
        }
    }
} 