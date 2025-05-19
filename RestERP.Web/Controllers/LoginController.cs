using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestERP.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace RestERP.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı adı ve şifre gereklidir.");
                    return View("Index");
                }

                var user = await _userManager.FindByNameAsync(username);
                if (user == null || !user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                    return View("Index");
                }

                var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyin.");
                    return View("Index");
                }

                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Giriş işlemi sırasında hata oluştu. Kullanıcı: {username}");
                ModelState.AddModelError(string.Empty, "Giriş işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete("JWT");
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string username, string email, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Şifreler eşleşmiyor.");
                    return View();
                }

                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    FirstName = null,
                    LastName = null,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View();
            }
            catch (Exception ex)
            {
                // Hata detaylarını logla
                _logger.LogError(ex, "Kayıt işlemi sırasında hata oluştu");
                
                // Kullanıcıya genel bir hata mesajı göster
                ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                return View();
            }
        }
    }
}
