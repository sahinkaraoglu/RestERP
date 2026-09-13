using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestERP.Core.Domain.Entities;
using RestERP.Application.Services.Abstract;

namespace RestERP.Web.Controllers
{
    [AllowAnonymous]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITableService _tableService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(
            IReservationService reservationService,
            ITableService tableService,
            ILogger<ReservationController> logger)
        {
            _reservationService = reservationService;
            _tableService = tableService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var tables = (await _tableService.GetAllTablesAsync()).ToList();
                var reservations = await _reservationService.GetAllReservationsAsync();
                ViewBag.Reservations = reservations;
                return View(tables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon sayfası yüklenirken hata oluştu");
                ViewBag.Reservations = new List<Reservation>();
                return View(new List<Table>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name, string phone, string date, string time, int guests, string notes)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime reservationDate))
                {
                    TempData["ErrorMessage"] = "Geçersiz tarih formatı.";
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrWhiteSpace(time))
                {
                    TempData["ErrorMessage"] = "Lütfen rezervasyon saatini seçiniz.";
                    return RedirectToAction("Index");
                }

                var rezervasyon = new Reservation
                {
                    Name = name?.Trim(),
                    Phone = phone?.Trim(),
                    Date = reservationDate,
                    Time = time.Trim(),
                    Guests = guests,
                    Notes = notes?.Trim()
                };

                await _reservationService.CreateReservationAsync(rezervasyon);
                TempData["SuccessMessage"] = "Rezervasyon talebiniz başarıyla kaydedildi.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon oluşturulurken hata oluştu");
                TempData["ErrorMessage"] = "Rezervasyon oluşturulurken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> List()
        {
            try
            {
                var reservations = await _reservationService.GetAllReservationsAsync();

                if (!reservations.Any())
                {
                    TempData["InfoMessage"] = "Henüz kayıtlı rezervasyon bulunmamaktadır.";
                }

                return View(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyonlar listelenirken hata oluştu");
                TempData["ErrorMessage"] = "Rezervasyonlar listelenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    TempData["ErrorMessage"] = "Silinecek rezervasyon bulunamadı.";
                }
                else
                {
                    await _reservationService.DeleteReservationAsync(id);
                    TempData["SuccessMessage"] = "Rezervasyon başarıyla silindi.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon silinirken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = "Rezervasyon silinirken bir hata oluştu.";
            }

            return RedirectToAction("List");
        }
    }
}
