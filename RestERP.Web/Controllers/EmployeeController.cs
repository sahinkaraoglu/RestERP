using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RestERP.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(
            IEmployeeService employeeService, 
            IUserService userService,
            ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                var users = await _userService.GetAllUsersAsync();
                
                ViewBag.Users = users; // Kullanıcı kayıtlarını ViewBag ile gönderiyoruz
                
                return View("~/Views/Panel/Employee/Index.cshtml", employees);
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
            return View();
        }

        public IActionResult EmployeeAdd()
        {
            return View("~/Views/Panel/Employee/EmployeeAdd.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeAdd(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Panel/Employee/EmployeeAdd.cshtml", employee);
                }

                await _employeeService.CreateEmployeeAsync(employee);
                TempData["SuccessMessage"] = "Personel başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel oluşturulurken hata oluştu");
                ModelState.AddModelError("", "Personel oluşturulurken bir hata oluştu: " + ex.Message);
                return View("~/Views/Panel/Employee/EmployeeAdd.cshtml", employee);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(employee);
                }

                await _employeeService.CreateEmployeeAsync(employee);
                TempData["SuccessMessage"] = "Personel başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel oluşturulurken hata oluştu");
                ModelState.AddModelError("", "Personel oluşturulurken bir hata oluştu: " + ex.Message);
                return View(employee);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Personel bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel düzenleme sayfası açılırken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Personel düzenleme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> EmployeeUpdate(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Personel bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                return View("~/Views/Panel/Employee/EmployeeUpdate.cshtml", employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel düzenleme sayfası açılırken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Personel düzenleme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeUpdate(int id, Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    TempData["ErrorMessage"] = "ID uyuşmazlığı.";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Panel/Employee/EmployeeUpdate.cshtml", employee);
                }

                var result = await _employeeService.UpdateEmployeeAsync(employee);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Personel güncellenirken bir hata oluştu.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Personel başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel güncellenirken hata oluştu. Id: {Id}", id);
                ModelState.AddModelError("", "Personel güncellenirken bir hata oluştu: " + ex.Message);
                return View("~/Views/Panel/Employee/EmployeeUpdate.cshtml", employee);
            }
        }

        public async Task<IActionResult> EmployeeDelete(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Personel bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                return View("~/Views/Panel/Employee/EmployeeDelete.cshtml", employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel silme sayfası açılırken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Personel silme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeDelete(int id, Employee employee)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(id);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Personel silinirken bir hata oluştu.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Personel başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel silinirken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Personel silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    TempData["ErrorMessage"] = "ID uyuşmazlığı.";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid)
                {
                    return View(employee);
                }

                var result = await _employeeService.UpdateEmployeeAsync(employee);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Personel güncellenirken bir hata oluştu.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Personel başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel güncellenirken hata oluştu. Id: {Id}", id);
                ModelState.AddModelError("", "Personel güncellenirken bir hata oluştu: " + ex.Message);
                return View(employee);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    TempData["ErrorMessage"] = "Personel bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel silme sayfası açılırken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Personel silme sayfası açılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(id);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Personel silinirken bir hata oluştu.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Personel başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Personel silinirken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Personel silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
