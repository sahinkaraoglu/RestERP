using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.Users.Commands.CreateUser;
using RestERP.Application.Features.Users.Commands.DeleteUser;
using RestERP.Application.Features.Users.Commands.ResetPassword;
using RestERP.Application.Features.Users.Commands.UpdateUser;
using RestERP.Application.Features.Users.Queries.GetUserById;
using RestERP.Application.Features.Users.Queries.GetUsers;
using RestERP.Domain.Enums;
using RestERP.Core.Domain.Entities;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(
            ILogger<UserController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = (await _mediator.Send(new GetUsersQuery())).ToList();
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel listesi alınırken hata oluştu");
                TempData["ErrorMessage"] = "Personel listesi alınırken bir hata oluştu: " + ex.Message;
                return View("Error", new RestERP.Web.Models.ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
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
                    user.RoleType = Role.Employee;

                    var result = await _mediator.Send(new CreateUserCommand(user, password));
                    var succeeded = result.Succeeded;
                    var errors = result.Errors;
                    if (succeeded)
                    {
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi.";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in errors)
                    {
                        ModelState.AddModelError("", error);
                    }
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

                var user = await _mediator.Send(new GetUserByIdQuery(id));
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
        public async Task<IActionResult> Edit(int id, ApplicationUser model, string? newPassword, string? confirmPassword)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Geçersiz kullanıcı ID'si.";
                    return RedirectToAction(nameof(Index));
                }

                var user = await _mediator.Send(new GetUserByIdQuery(id));
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                var resetPassword = !string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword);
                if (resetPassword)
                {
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
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.IsActive = model.IsActive;
                user.RoleType = model.RoleType;

                var updated = await _mediator.Send(new UpdateUserCommand(user));
                if (!updated)
                {
                    TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir hata oluştu.";
                    return View("~/Areas/Admin/Views/User/Edit.cshtml", model);
                }

                if (resetPassword)
                {
                    var resetResult = await _mediator.Send(new ResetPasswordCommand(id, newPassword!));
                    var resetSucceeded = resetResult.Succeeded;
                    var resetErrors = resetResult.Errors;
                    if (!resetSucceeded)
                    {
                        TempData["ErrorMessage"] = "Kullanıcı güncellendi ancak şifre sıfırlanamadı. " +
                            string.Join(" ", resetErrors);
                        return View("~/Areas/Admin/Views/User/Edit.cshtml", model);
                    }

                    TempData["SuccessMessage"] = "Kullanıcı güncellendi ve şifre sıfırlandı.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
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

                var deleted = await _mediator.Send(new DeleteUserCommand(id));
                if (deleted)
                {
                    return Json(new { success = true, message = "Kullanıcı başarıyla silindi." });
                }

                return Json(new { success = false, message = "Kullanıcı silinirken bir hata oluştu." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken hata oluştu. Id: {Id}", id);
                return Json(new { success = false, message = "Kullanıcı silinirken bir hata oluştu: " + ex.Message });
            }
        }
    }
}
