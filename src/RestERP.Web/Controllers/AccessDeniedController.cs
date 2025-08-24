using Microsoft.AspNetCore.Mvc;

namespace RestERP.Web.Controllers
{
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 