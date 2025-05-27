using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Enums;
using RestERP.Core.Doman.Entities;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PersonController(
            ILogger<PersonController> logger, 
            IUserService userService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Tüm kullanıcıları çekelim
                var users = await _userService.GetAllUsersAsync();
                
                // Kullanıcıları doğrudan model olarak View'a gönderelim
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel listesi alınırken hata oluştu");
                TempData["ErrorMessage"] = "Personel listesi alınırken bir hata oluştu: " + ex.Message;
                return View("Error");
            }
        }

        // GET: Person/PersonAdd
        public IActionResult PersonAdd()
        {
            return View("~/Views/Panel/Person/PersonAdd.cshtml");
        }

        // POST: Person/PersonAdd
        [HttpPost]
        public async Task<IActionResult> PersonAdd(ApplicationUser user, string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError("", "Şifreler eşleşmiyor!");
                    return View("~/Views/Panel/Person/PersonAdd.cshtml", user);
                }

                if (ModelState.IsValid)
                {
                    // Kullanıcı adını e-posta adresinden oluştur
                    user.UserName = user.Email;
                    user.IsActive = true;

                    var result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Kullanıcı  başarıyla eklendi.";
                        return RedirectToAction(nameof(Index));
                    }
                    
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View("~/Views/Panel/Person/PersonAdd.cshtml", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı eklenirken hata oluştu");
                TempData["ErrorMessage"] = "Kullanıcı eklenirken bir hata oluştu: " + ex.Message;
                return View("~/Views/Panel/Person/PersonAdd.cshtml", user);
            }
        }

        // GET: Person/PersonUpdate
        public async Task<IActionResult> PersonUpdate(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
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
            
                return View("~/Views/Panel/Person/PersonUpdate.cshtml", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncelleme sayfası açılırken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Kullanıcı güncelleme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Person/PersonUpdate
        [HttpPost]
        public async Task<IActionResult> PersonUpdate(string id, ApplicationUser model, string? currentPassword, string? newPassword, string? confirmPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
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
                        return View("~/Views/Panel/Person/PersonUpdate.cshtml", model);
                    }

                    if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
                    {
                        ModelState.AddModelError("", "Yeni şifre en az 6 karakter uzunluğunda olmalıdır.");
                        return View("~/Views/Panel/Person/PersonUpdate.cshtml", model);
                    }

                    if (newPassword != confirmPassword)
                    {
                        ModelState.AddModelError("", "Yeni şifreler eşleşmiyor.");
                        return View("~/Views/Panel/Person/PersonUpdate.cshtml", model);
                    }

                    // Mevcut şifreyi kontrol et
                    var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, currentPassword);
                    if (!isCurrentPasswordValid)
                    {
                        ModelState.AddModelError("", "Mevcut şifre yanlış.");
                        return View("~/Views/Panel/Person/PersonUpdate.cshtml", model);
                    }

                    // Şifreyi değiştir
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View("~/Views/Panel/Person/PersonUpdate.cshtml", model);
                    }
                }

                // Diğer bilgileri güncelle
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.IsActive = model.IsActive;
                user.RoleType = model.RoleType;
            
                var updateResult = await _userService.UpdateUserAsync(user);
                if (!updateResult)
                {
                    TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu.";
                    return View("~/Views/Panel/Person/PersonUpdate.cshtml", model);
                }
            
                TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu: " + ex.Message;
                return View("~/Views/Panel/Person/PersonUpdate.cshtml", model);
            }
        }

        // GET: Person/PersonDelete
        public async Task<IActionResult> PersonDelete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["ErrorMessage"] = "Geçersiz kullanıcı ID'si.";
                    return RedirectToAction(nameof(Index));
                }

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Silinecek kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                return View("~/Views/Panel/Person/PersonDelete.cshtml", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silme sayfası açılırken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Kullanıcı silme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Person/PersonDelete
        [HttpPost]
        [ActionName("PersonDelete")]
        public async Task<IActionResult> PersonDeleteConfirmed(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["ErrorMessage"] = "Geçersiz kullanıcı ID'si.";
                    return RedirectToAction(nameof(Index));
                }

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Silinecek kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Kullanıcı silinirken bir hata oluştu: " + string.Join(", ", result.Errors.Select(e => e.Description));
                    return View("~/Views/Panel/Person/PersonDelete.cshtml", user);
                }

                TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Kullanıcı silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
} 