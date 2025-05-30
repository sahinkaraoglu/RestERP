using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Core.Doman.Entities;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Enums;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Employee")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return View(reservations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _reservationService.CreateReservationAsync(reservation);
                    TempData["SuccessMessage"] = "Rezervasyon başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return View(reservation);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _reservationService.UpdateReservationAsync(reservation);
                    TempData["SuccessMessage"] = "Rezervasyon başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _reservationService.DeleteReservationAsync(id);
                TempData["SuccessMessage"] = "Rezervasyon başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Export(string format)
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            
            switch (format.ToLower())
            {
                case "excel":
                    // Excel export işlemi
                    return File(new byte[] { }, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reservations.xlsx");
                
                case "pdf":
                    // PDF export işlemi
                    return File(new byte[] { }, "application/pdf", "Reservations.pdf");
                
                case "csv":
                    // CSV export işlemi
                    return File(new byte[] { }, "text/csv", "Reservations.csv");
                
                default:
                    return BadRequest("Desteklenmeyen format.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Filter(DateTime? startDate, DateTime? endDate, string status)
        {
            var reservations = await _reservationService.GetAllReservationsAsync();

            if (startDate.HasValue)
            {
                reservations = reservations.Where(r => r.Date >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                reservations = reservations.Where(r => r.Date <= endDate.Value).ToList();
            }

            //if (!string.IsNullOrEmpty(status))
            //{
            //    reservations = reservations.Where(r => r.Status == status).ToList();
            //}

            return View("Index", reservations);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservationStats()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var todayCount = reservations.Count(r => r.Date.Date == today);
            var tomorrowCount = reservations.Count(r => r.Date.Date == tomorrow);

            return Json(new { todayCount, tomorrowCount });
        }
    }
} 