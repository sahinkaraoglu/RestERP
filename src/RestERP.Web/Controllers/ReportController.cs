using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.Orders.Queries.GetOrders;
using RestERP.Core.Domain.Entities;

namespace RestERP.Web.Controllers;

public class ReportController : Controller
{
    private readonly ILogger<ReportController> _logger;
    private readonly IMediator _mediator;

    public ReportController(
        ILogger<ReportController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var orders = (await _mediator.Send(new GetOrdersQuery())).ToList();
            return View(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Rapor sayfası yüklenirken hata oluştu");
            TempData["ErrorMessage"] = "Rapor sayfası yüklenirken bir hata oluştu: " + ex.Message;
            return View(new List<Order>());
        }
    }
}
