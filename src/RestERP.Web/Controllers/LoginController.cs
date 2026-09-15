using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.DTOs;
using RestERP.Application.Features.Auth.Commands.Login;
using RestERP.Application.Features.Auth.Commands.Register;

namespace RestERP.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IMediator mediator, ILogger<LoginController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError(string.Empty, "E-posta ve şifre gereklidir.");
                    return View("Index");
                }

                var tokenResponse = await _mediator.Send(new LoginCommand(new LoginRequest
                {
                    Email = email,
                    Password = password
                }));

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                    return View("Index");
                }

                SetAuthCookie(tokenResponse, rememberMe);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında hata oluştu");
                ModelState.AddModelError(string.Empty, "Giriş işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                return View("Index");
            }
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWT");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string username, string email, string firstName, string lastName, string phoneNumber, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Şifreler eşleşmiyor.");
                    return View();
                }

                var tokenResponse = await _mediator.Send(new RegisterCommand(new RegisterRequest
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    Password = password,
                    ConfirmPassword = confirmPassword
                }));

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulurken bir hata oluştu. Email veya kullanıcı adı zaten kullanılıyor olabilir.");
                    return View();
                }

                SetAuthCookie(tokenResponse, rememberMe: false);
                TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturulmuştur.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt işlemi sırasında hata oluştu. Kullanıcı: {Username}", username);
                ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private void SetAuthCookie(TokenResponse tokenResponse, bool rememberMe)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = rememberMe ? tokenResponse.ExpiresAt : null
            };

            Response.Cookies.Append("JWT", tokenResponse.AccessToken, cookieOptions);
        }
    }
}
