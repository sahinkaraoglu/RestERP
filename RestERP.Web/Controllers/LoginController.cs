using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RestERP.Core.Domain.Entities;
using RestERP.Application.Services.Interfaces;
using System.Security.Cryptography;

namespace RestERP.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(
            IUserService userService,
            IConfiguration configuration,
            ILogger<LoginController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            try
            {
                var key = _configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(key))
                {
                    throw new InvalidOperationException("JWT anahtarı yapılandırılmamış.");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ?? string.Empty),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim("FirstName", user.FirstName ?? string.Empty),
                    new Claim("LastName", user.LastName ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.RoleType.ToString())
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpiryInDays"] ?? "7"));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JWT token oluşturulurken hata oluştu");
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
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

                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null || !user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                    return View("Index");
                }

                if (!VerifyPassword(password, user.PasswordHash))
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                    return View("Index");
                }

                // JWT token oluştur
                var token = GenerateJwtToken(user);

                // Token'ı cookie'ye kaydet
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpiryInDays"] ?? "7"))
                };

                Response.Cookies.Append("JWT", token, cookieOptions);

                // Başarılı giriş sonrası Home/Index'e yönlendir
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında hata oluştu");
                ModelState.AddModelError(string.Empty, "Giriş işlemi sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
                return View("Index");
            }
        }

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

                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    IsActive = true,
                    PasswordHash = HashPassword(password),
                    RoleType = RestERP.Domain.Enums.Role.Customer,
                    CreatedDate = DateTime.Now
                };

                var result = await _userService.CreateUserAsync(user);
                if (result)
                {
                    // JWT token oluştur
                    var token = GenerateJwtToken(user);

                    // Token'ı cookie'ye kaydet
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpiryInDays"] ?? "7"))
                    };

                    Response.Cookies.Append("JWT", token, cookieOptions);

                    TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturulmuştur.";
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulurken bir hata oluştu.");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Kayıt işlemi sırasında hata oluştu. Kullanıcı: {username}");
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
    }
}
