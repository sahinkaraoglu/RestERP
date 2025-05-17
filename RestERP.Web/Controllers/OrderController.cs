using Microsoft.AspNetCore.Mvc;
using RestERP.Domain.Entities;

namespace RestERP.Web.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Save(Order order)
    {
        return RedirectToAction("Index", "Home");
    }
} 