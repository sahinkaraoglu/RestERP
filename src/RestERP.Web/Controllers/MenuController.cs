using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Web.Models;

namespace RestERP.Web.Controllers;

public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;
    private readonly IFoodService _foodService;
    private readonly IUserService _userService;

    public MenuController(
        ILogger<MenuController> logger,
        IFoodService foodService,
        IUserService userService)
    {
        _logger = logger;
        _foodService = foodService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = (await _foodService.GetAllFoodCategoriesAsync()).ToList();
            var foods = (await _foodService.GetAllFoodsAsync()).ToList();
            var images = (await _foodService.GetAllFoodImagesAsync()).ToList();

            ViewBag.Categories = categories;
            ViewBag.Foods = foods;
            ViewBag.Images = images;

            if (User.Identity?.IsAuthenticated == true)
            {
                ViewBag.CurrentUser = await ResolveCurrentUserAsync();
            }

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Menü sayfası yüklenirken hata oluştu");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    private async Task<RestERP.Core.Domain.Entities.ApplicationUser?> ResolveCurrentUserAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userId, out var id) && id > 0)
        {
            var byId = await _userService.GetUserByIdAsync(id);
            if (byId != null)
            {
                return byId;
            }
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var byEmail = await _userService.GetUserByEmailAsync(email);
            if (byEmail != null)
            {
                return byEmail;
            }
        }

        var name = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        return await _userService.GetUserByUsernameAsync(name)
            ?? await _userService.GetUserByEmailAsync(name);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
