using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestERP.Web.Models;

namespace RestERP.Web.Controllers;

public class PanelController : Controller
{
    private readonly ILogger<PanelController> _logger;

    public PanelController(ILogger<PanelController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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
