using Microsoft.AspNetCore.Mvc;

namespace RestERP.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [HttpPost]
    public IActionResult Logout()
    {
        _logger.LogInformation("User logging out from Admin area");
        
        // JWT cookie'yi sil
        Response.Cookies.Delete("JWT");
        
        // Ana sayfaya yönlendir
        return RedirectToAction("Index", "Home", new { area = "" });
    }
}

