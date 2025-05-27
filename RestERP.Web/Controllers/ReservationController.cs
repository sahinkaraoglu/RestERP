using Microsoft.AspNetCore.Mvc;
using RestERP.Core.Doman.Entities;
using RestERP.Infrastructure;

namespace RestERP.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly RestERPDbContext _context;

        public ReservationController(RestERPDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tables = _context.Tables.ToList();
            return View(tables);
        }
    }
} 