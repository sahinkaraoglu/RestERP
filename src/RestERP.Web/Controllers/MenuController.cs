using System.Diagnostics;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.FoodCategories.Queries.GetFoodCategories;
using RestERP.Application.Features.Foods.Queries.GetFoodImages;
using RestERP.Application.Features.Foods.Queries.GetFoods;
using RestERP.Application.Features.Users.Queries.GetUserByEmail;
using RestERP.Application.Features.Users.Queries.GetUserById;
using RestERP.Application.Features.Users.Queries.GetUserByUsername;
using RestERP.Web.Models;

namespace RestERP.Web.Controllers;

public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;
    private readonly IMediator _mediator;

    public MenuController(
        ILogger<MenuController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = (await _mediator.Send(new GetFoodCategoriesQuery())).ToList();
            var foods = (await _mediator.Send(new GetFoodsQuery())).ToList();
            var images = (await _mediator.Send(new GetFoodImagesQuery())).ToList();

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
            var byId = await _mediator.Send(new GetUserByIdQuery(id));
            if (byId != null)
            {
                return byId;
            }
        }

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (!string.IsNullOrWhiteSpace(email))
        {
            var byEmail = await _mediator.Send(new GetUserByEmailQuery(email));
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

        return await _mediator.Send(new GetUserByUsernameQuery(name))
            ?? await _mediator.Send(new GetUserByEmailQuery(name));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
