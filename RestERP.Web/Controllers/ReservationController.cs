using Microsoft.AspNetCore.Mvc;
using RestERP.Core.Doman.Entities;
using RestERP.Infrastructure;
using RestERP.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace RestERP.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly RestERPDbContext _context;
        private readonly IReservationService _reservationService;

        public ReservationController(RestERPDbContext context, IReservationService reservationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        }

        public async Task<IActionResult> Index()
        {
            var tables = await Task.FromResult(_context.Tables.ToList());
            ViewBag.Reservations = await _reservationService.GetAllReservationsAsync();
            return View(tables);
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

                var createdReservation = await _reservationService.CreateReservationAsync(rezervasyon);
                
                // Rezervasyonun başarıyla kaydedildiğini kontrol et
                var savedReservation = await _reservationService.GetReservationByIdAsync(createdReservation.Id);
                if (savedReservation != null)
                {
                    TempData["SuccessMessage"] = "Rezervasyon talebiniz başarıyla kaydedildi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Rezervasyon kaydedilirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Rezervasyon kaydedilirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Rezervasyon oluşturulurken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
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
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Rezervasyonlar listelenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _reservationService.DeleteReservationAsync(id);
                TempData["SuccessMessage"] = "Rezervasyon başarıyla silindi.";
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Silinecek rezervasyon bulunamadı.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Rezervasyon silinirken bir hata oluştu.";
            }

            return RedirectToAction("List");
        }
    }
} 