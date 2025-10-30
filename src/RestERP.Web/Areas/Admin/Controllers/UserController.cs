using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Domain.Enums;
using RestERP.Core.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(
            ILogger<UserController> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var response = await httpClient.GetAsync("api/user");
                
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Personel listesi alınırken bir hata oluştu.";
                    return View(new List<ApplicationUser>());
                }

                var json = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<ApplicationUser>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(users ?? new List<ApplicationUser>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel listesi alınırken hata oluştu");
                TempData["ErrorMessage"] = "Personel listesi alınırken bir hata oluştu: " + ex.Message;
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            return View("~/Areas/Admin/Views/User/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError("", "Şifreler eşleşmiyor!");
                    return View("~/Areas/Admin/Views/User/Create.cshtml", user);
                }

                if (ModelState.IsValid)
                {
                    user.UserName = user.Email;
                    user.IsActive = true;
                    user.PasswordHash = HashPassword(password);
                    user.RoleType = Role.Employee;
                    user.PhoneNumber = user.PhoneNumber;
                    user.CreatedDate = DateTime.Now;

                    var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                    var json = JsonSerializer.Serialize(user);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("api/user", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi.";
                        return RedirectToAction(nameof(Index));
                    }
                    
                    ModelState.AddModelError("", "Kullanıcı eklenirken bir hata oluştu.");
                }
                return View("~/Areas/Admin/Views/User/Create.cshtml", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı eklenirken hata oluştu");
                TempData["ErrorMessage"] = "Kullanıcı eklenirken bir hata oluştu: " + ex.Message;
                return View("~/Areas/Admin/Views/User/Create.cshtml", user);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Geçersiz kullanıcı ID'si.";
                    return RedirectToAction(nameof(Index));
                }

                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var response = await httpClient.GetAsync($"api/user/{id}");
                
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<ApplicationUser>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                return View("~/Areas/Admin/Views/User/Edit.cshtml", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncelleme sayfası açılırken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Kullanıcı güncelleme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ApplicationUser model, string? currentPassword, string? newPassword, string? confirmPassword)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Geçersiz kullanıcı ID'si.";
                    return RedirectToAction(nameof(Index));
                }

                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var getResponse = await httpClient.GetAsync($"api/user/{id}");
                
                if (!getResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                var json = await getResponse.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<ApplicationUser>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                // Şifre değişikliği kontrolü
                if (!string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword))
                {
                    if (string.IsNullOrEmpty(currentPassword))
                    {
                        ModelState.AddModelError("", "Mevcut şifrenizi girmelisiniz.");
                        return View("~/Areas/Admin/Views/User/Edit.cshtml", model);
                    }

                    if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
                    {
                        ModelState.AddModelError("", "Yeni şifre en az 6 karakter uzunluğunda olmalıdır.");
                        return View("~/Areas/Admin/Views/User/Edit.cshtml", model);
                    }

                    if (newPassword != confirmPassword)
                    {
                        ModelState.AddModelError("", "Yeni şifreler eşleşmiyor.");
                        return View("~/Areas/Admin/Views/User/Edit.cshtml", model);
                    }

                    // Mevcut şifreyi kontrol et
                    if (!VerifyPassword(currentPassword, user.PasswordHash))
                    {
                        ModelState.AddModelError("", "Mevcut şifre yanlış.");
                        return View("~/Areas/Admin/Views/User/Edit.cshtml", model);
                    }

                    // Şifreyi güncelle
                    user.PasswordHash = HashPassword(newPassword);
                }

                // Diğer bilgileri güncelle
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.IsActive = model.IsActive;
                user.RoleType = model.RoleType;
                user.CreatedDate = model.CreatedDate;
                user.UpdatedDate = model.UpdatedDate;
            
                var updateJson = JsonSerializer.Serialize(user);
                var content = new StringContent(updateJson, Encoding.UTF8, "application/json");
                var updateResponse = await httpClient.PutAsync($"api/user/{id}", content);
                
                if (updateResponse.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu.";
                    return View("~/Areas/Admin/Views/User/Edit.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu: " + ex.Message;
                return View("~/Areas/Admin/Views/User/Edit.cshtml", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Json(new { success = false, message = "Geçersiz kullanıcı ID'si." });
                }

                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var response = await httpClient.DeleteAsync($"api/user/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Kullanıcı başarıyla silindi." });
                }
                else
                {
                    return Json(new { success = false, message = "Kullanıcı silinirken bir hata oluştu." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken hata oluştu. Id: {Id}", id);
                return Json(new { success = false, message = "Kullanıcı silinirken bir hata oluştu: " + ex.Message });
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
    }
} 

