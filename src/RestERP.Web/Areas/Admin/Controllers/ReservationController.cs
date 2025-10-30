using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Core.Domain.Entities;
using RestERP.Application.Services.Abstract;
using RestERP.Domain.Enums;
using System.Text;
using System.Text.Json;

namespace RestERP.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class ReservationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReservationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync("api/reservation");
            
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Reservation>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var reservations = JsonSerializer.Deserialize<List<Reservation>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(reservations ?? new List<Reservation>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync($"api/reservation/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var reservation = JsonSerializer.Deserialize<Reservation>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
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
                    var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                    var json = JsonSerializer.Serialize(reservation);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("api/reservation", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Rezervasyon başarıyla oluşturuldu.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Rezervasyon oluşturulurken bir hata oluştu.";
                    }
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync($"api/reservation/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var reservation = JsonSerializer.Deserialize<Reservation>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
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
                    var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                    var json = JsonSerializer.Serialize(reservation);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync($"api/reservation/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Rezervasyon başarıyla güncellendi.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Rezervasyon güncellenirken bir hata oluştu.";
                    }
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
                var httpClient = _httpClientFactory.CreateClient("RestERPApi");
                var response = await httpClient.DeleteAsync($"api/reservation/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Rezervasyon başarıyla silindi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Rezervasyon silinirken bir hata oluştu.";
                }
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync("api/reservation");
            
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Rezervasyonlar alınırken bir hata oluştu.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var reservations = JsonSerializer.Deserialize<List<Reservation>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync("api/reservation");
            
            if (!response.IsSuccessStatusCode)
            {
                return View("Index", new List<Reservation>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var reservations = JsonSerializer.Deserialize<List<Reservation>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Reservation>();

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
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync("api/reservation");
            
            if (!response.IsSuccessStatusCode)
            {
                return Json(new { todayCount = 0, tomorrowCount = 0 });
            }

            var json = await response.Content.ReadAsStringAsync();
            var reservations = JsonSerializer.Deserialize<List<Reservation>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Reservation>();
            
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var todayCount = reservations.Count(r => r.Date.Date == today);
            var tomorrowCount = reservations.Count(r => r.Date.Date == tomorrow);

            return Json(new { todayCount, tomorrowCount });
        }
    }
} 