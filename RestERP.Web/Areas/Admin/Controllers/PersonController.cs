using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Enums;
using RestERP.Core.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IUserService _userService;

        public PersonController(
            ILogger<PersonController> logger, 
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return View(users);
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
            return View("~/Areas/Admin/Views/Person/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError("", "Şifreler eşleşmiyor!");
                    return View("~/Areas/Admin/Views/Person/Create.cshtml", user);
                }

                if (ModelState.IsValid)
                {
                    user.UserName = user.Email;
                    user.IsActive = true;
                    user.PasswordHash = HashPassword(password);
                    user.RoleType = Role.Employee;
                    user.PhoneNumber = user.PhoneNumber;

                    var result = await _userService.CreateUserAsync(user);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi.";
                        return RedirectToAction(nameof(Index));
                    }
                    
                    ModelState.AddModelError("", "Kullanıcı eklenirken bir hata oluştu.");
                }
                return View("~/Areas/Admin/Views/Person/Create.cshtml", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı eklenirken hata oluştu");
                TempData["ErrorMessage"] = "Kullanıcı eklenirken bir hata oluştu: " + ex.Message;
                return View("~/Areas/Admin/Views/Person/Create.cshtml", user);
            }
        }

        public async Task<IActionResult> PersonUpdate(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Geçersiz kullanıcı ID'si.";
                    return RedirectToAction(nameof(Index));
                }

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                return View("~/Areas/Admin/Views/Person/PersonUpdate.cshtml", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncelleme sayfası açılırken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Kullanıcı güncelleme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PersonUpdate(int id, ApplicationUser model, string? currentPassword, string? newPassword, string? confirmPassword)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Geçersiz kullanıcı ID'si.";
                    return RedirectToAction(nameof(Index));
                }

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                // Şifre değişikliği kontrolü
                if (!string.IsNullOrEmpty(currentPassword) || !string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword))
                {
                    if (string.IsNullOrEmpty(currentPassword))
                    {
                        ModelState.AddModelError("", "Mevcut şifrenizi girmelisiniz.");
                        return View("~/Areas/Admin/Views/Person/PersonUpdate.cshtml", model);
                    }

                    if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
                    {
                        ModelState.AddModelError("", "Yeni şifre en az 6 karakter uzunluğunda olmalıdır.");
                        return View("~/Areas/Admin/Views/Person/PersonUpdate.cshtml", model);
                    }

                    if (newPassword != confirmPassword)
                    {
                        ModelState.AddModelError("", "Yeni şifreler eşleşmiyor.");
                        return View("~/Areas/Admin/Views/Person/PersonUpdate.cshtml", model);
                    }

                    // Mevcut şifreyi kontrol et
                    if (!VerifyPassword(currentPassword, user.PasswordHash))
                    {
                        ModelState.AddModelError("", "Mevcut şifre yanlış.");
                        return View("~/Areas/Admin/Views/Person/PersonUpdate.cshtml", model);
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
            
                var updateResult = await _userService.UpdateUserAsync(user);
                if (!updateResult)
                {
                    TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu.";
                    return View("~/Areas/Admin/Views/Person/PersonUpdate.cshtml", model);
                }
            
                TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu: " + ex.Message;
                return View("~/Areas/Admin/Views/Person/PersonUpdate.cshtml", model);
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

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "Silinecek kullanıcı bulunamadı." });
                }

                var result = await _userService.DeleteUserAsync(id);
                if (result)
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