using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using RestERP.Application.DTOs;

namespace RestERP.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<LoginController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient("RestERPApi");
            return client;
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
                _logger.LogInformation($"Login attempt for email: {email}");
                
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    _logger.LogWarning("Email or password is empty");
                    ModelState.AddModelError(string.Empty, "E-posta ve şifre gereklidir.");
                    return View("Index");
                }

                // API'ye login request gönder
                var client = CreateHttpClient();
                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                var jsonContent = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("api/auth/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("API login failed for email: {Email}. Status: {StatusCode}, Error: {Error}", email, response.StatusCode, errorContent);
                    ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                    return View("Index");
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseJson, options);

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    _logger.LogError("Token response is null or empty");
                    ModelState.AddModelError(string.Empty, "Giriş işlemi sırasında bir hata oluştu.");
                    return View("Index");
                }

                // Token'ı cookie'ye kaydet
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = tokenResponse.ExpiresAt
                };

                Response.Cookies.Append("JWT", tokenResponse.AccessToken, cookieOptions);

                _logger.LogInformation($"Login successful for email: {email}");
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

        [HttpGet]
        [HttpPost]
        public IActionResult Logout()
        {
            _logger.LogInformation("User logging out");
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

                // API'ye register request gönder
                var client = CreateHttpClient();
                var registerRequest = new RegisterRequest
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    Password = password,
                    ConfirmPassword = confirmPassword
                };

                var jsonContent = JsonSerializer.Serialize(registerRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("api/auth/register", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API register failed for user: {Username}. Status: {StatusCode}, Error: {Error}", username, response.StatusCode, errorContent);
                    ModelState.AddModelError(string.Empty, "Kullanıcı oluşturulurken bir hata oluştu. Email veya kullanıcı adı zaten kullanılıyor olabilir.");
                    return View();
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseJson, options);

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    _logger.LogError("Token response is null or empty");
                    ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu.");
                    return View();
                }

                // Token'ı cookie'ye kaydet
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = tokenResponse.ExpiresAt
                };

                Response.Cookies.Append("JWT", tokenResponse.AccessToken, cookieOptions);

                TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturulmuştur.";
                return RedirectToAction("Index", "Home");
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
