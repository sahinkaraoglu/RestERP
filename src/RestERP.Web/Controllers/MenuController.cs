using System.Diagnostics;
using System.Security.Claims;
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

            if (User.Identity?.IsAuthenticated == true)
            {
                try
                {
                    ViewBag.CurrentUser = await ResolveCurrentUserAsync(client);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Kullanıcı bilgisi alınırken hata oluştu");
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


    private async Task<RestERP.Core.Domain.Entities.ApplicationUser?> ResolveCurrentUserAsync(HttpClient client)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        async Task<RestERP.Core.Domain.Entities.ApplicationUser?> TryGetAsync(string url)
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<RestERP.Core.Domain.Entities.ApplicationUser>(json, options);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userId, out var id) && id > 0)
        {
            var byId = await TryGetAsync($"api/user/{id}");
            if (byId != null)
            {
                return byId;
            }
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var byEmail = await TryGetAsync($"api/user/email/{Uri.EscapeDataString(email)}");
            if (byEmail != null)
            {
                return byEmail;
            }
        }

        var name = User.Identity?.Name;
        if (!string.IsNullOrWhiteSpace(name))
        {
            var byUsername = await TryGetAsync($"api/user/username/{Uri.EscapeDataString(name)}");
            if (byUsername != null)
            {
                return byUsername;
            }

            return await TryGetAsync($"api/user/email/{Uri.EscapeDataString(name)}");
        }

        return null;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}