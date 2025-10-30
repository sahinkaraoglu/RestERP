using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Web.Models;
using System.Text.Json;

namespace RestERP.Web.Controllers;

public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public MenuController(ILogger<MenuController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("RestERPApi");
            var categoriesTask = client.GetAsync("api/food/categories");
            var foodsTask = client.GetAsync("api/food");
            var imagesTask = client.GetAsync("api/food/images");

            await Task.WhenAll(categoriesTask, foodsTask, imagesTask);

            if (!categoriesTask.Result.IsSuccessStatusCode || !foodsTask.Result.IsSuccessStatusCode || !imagesTask.Result.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var categoriesJson = await categoriesTask.Result.Content.ReadAsStringAsync();
            var foodsJson = await foodsTask.Result.Content.ReadAsStringAsync();
            var imagesJson = await imagesTask.Result.Content.ReadAsStringAsync();

            var categories = JsonSerializer.Deserialize<List<RestERP.Core.Domain.Entities.FoodCategory>>(categoriesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<RestERP.Core.Domain.Entities.FoodCategory>();
            var foods = JsonSerializer.Deserialize<List<RestERP.Core.Domain.Entities.Food>>(foodsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<RestERP.Core.Domain.Entities.Food>();
            var images = JsonSerializer.Deserialize<List<RestERP.Core.Domain.Entities.Image>>(imagesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<RestERP.Core.Domain.Entities.Image>();

            ViewBag.Categories = categories;
            ViewBag.Foods = foods;
            ViewBag.Images = images;

            // Kullanıcı giriş yaptıysa API'den bilgilerini çek
            if (User.Identity?.IsAuthenticated == true)
            {
                var userEmail = User.Identity.Name;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    try
                    {
                        var userResponse = await client.GetAsync($"api/user/email/{Uri.EscapeDataString(userEmail)}");
                        if (userResponse.IsSuccessStatusCode)
                        {
                            var userJson = await userResponse.Content.ReadAsStringAsync();
                            var currentUser = JsonSerializer.Deserialize<RestERP.Core.Domain.Entities.ApplicationUser>(userJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            ViewBag.CurrentUser = currentUser;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Kullanıcı bilgisi alınırken hata oluştu: {Email}", userEmail);
                    }
                }
            }

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Menü sayfası yüklenirken hata oluştu");
            return View("Error");
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}