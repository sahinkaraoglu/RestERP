using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;
using RestERP.Domain.Enums;

namespace RestERP.Web.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IUserService _userService;

        public PersonController(ILogger<PersonController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Tüm kullanıcıları çekelim
                var users = await _userService.GetAllUsersAsync();
                
                // Kullanıcıları doğrudan model olarak View'a gönderelim
                return View("~/Views/Panel/Person/Index.cshtml", users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel listesi alınırken hata oluştu");
                TempData["ErrorMessage"] = "Personel listesi alınırken bir hata oluştu: " + ex.Message;
                return View("Error");
            }
        }
    }
} 