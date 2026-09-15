using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using RestERP.Core.Domain.Entities;
using RestERP.Application.Features.Reservations.Commands.CreateReservation;
using RestERP.Application.Features.Reservations.Commands.DeleteReservation;
using RestERP.Application.Features.Reservations.Commands.UpdateReservation;
using RestERP.Application.Features.Reservations.Queries.GetReservationById;
using RestERP.Application.Features.Reservations.Queries.GetReservations;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class ReservationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(
            IMediator mediator,
            ILogger<ReservationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var reservations = await _mediator.Send(new GetReservationsQuery());
                return View(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon listesi alınırken hata oluştu");
                TempData["ErrorMessage"] = "Rezervasyon listesi alınırken bir hata oluştu.";
                return View(new List<Reservation>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _mediator.Send(new GetReservationByIdQuery(id));
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
                    await _mediator.Send(new CreateReservationCommand(reservation));
                    TempData["SuccessMessage"] = "Rezervasyon başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Rezervasyon oluşturulurken hata oluştu");
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            return View(reservation);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _mediator.Send(new GetReservationByIdQuery(id));
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
                    await _mediator.Send(new UpdateReservationCommand(reservation));
                    TempData["SuccessMessage"] = "Rezervasyon başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Rezervasyon güncellenirken hata oluştu. Id: {Id}", id);
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
                await _mediator.Send(new DeleteReservationCommand(id));
                TempData["SuccessMessage"] = "Rezervasyon başarıyla silindi.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon silinirken hata oluştu. Id: {Id}", id);
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Export(string format)
        {
            var reservations = await _mediator.Send(new GetReservationsQuery());

            switch (format.ToLower())
            {
                case "excel":
                    return File(Array.Empty<byte>(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reservations.xlsx");

                case "pdf":
                    return File(Array.Empty<byte>(), "application/pdf", "Reservations.pdf");

                case "csv":
                    return File(Array.Empty<byte>(), "text/csv", "Reservations.csv");

                default:
                    return BadRequest("Desteklenmeyen format.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Filter(DateTime? startDate, DateTime? endDate, string status)
        {
            var reservations = await _mediator.Send(new GetReservationsQuery());

            if (startDate.HasValue)
            {
                reservations = reservations.Where(r => r.Date >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                reservations = reservations.Where(r => r.Date <= endDate.Value).ToList();
            }

            return View("Index", reservations);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservationStats()
        {
            var reservations = await _mediator.Send(new GetReservationsQuery());

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var todayCount = reservations.Count(r => r.Date.Date == today);
            var tomorrowCount = reservations.Count(r => r.Date.Date == tomorrow);

            return Json(new { todayCount, tomorrowCount });
        }
    }
}
