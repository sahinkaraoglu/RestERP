using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestERP.Core.Domain.Entities;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.Net.Http.Headers;

namespace RestERP.Web.Controllers
{
    [AllowAnonymous]
    public class ReservationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(
            IHttpClientFactory httpClientFactory,
            ILogger<ReservationController> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient("RestERPApi");
            
            // JWT token'ı cookie'den al ve header'a ekle
            var token = Request.Cookies["JWT"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            return client;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // API'den masaları getir
                var client = CreateHttpClient();
                var tablesResponse = await client.GetAsync("api/table");
                List<Table> tables = new List<Table>();
                if (tablesResponse.IsSuccessStatusCode)
                {
                    var tablesJson = await tablesResponse.Content.ReadAsStringAsync();
                    var tablesOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    tables = JsonSerializer.Deserialize<List<Table>>(tablesJson, tablesOptions) ?? new List<Table>();
                }
                else
                {
                    _logger.LogWarning("API'den masalar alınamadı. Status: {StatusCode}", tablesResponse.StatusCode);
                }

                // API'den rezervasyonları getir
                var response = await client.GetAsync("api/reservation");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var reservations = JsonSerializer.Deserialize<List<Reservation>>(json, options) ?? new List<Reservation>();
                    ViewBag.Reservations = reservations;
                }
                else
                {
                    _logger.LogWarning("API'den rezervasyonlar alınamadı. Status: {StatusCode}", response.StatusCode);
                    ViewBag.Reservations = new List<Reservation>();
                }

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

                // API'ye rezervasyon gönder
                var client = CreateHttpClient();
                var jsonContent = JsonSerializer.Serialize(rezervasyon);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("api/reservation", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Rezervasyon talebiniz başarıyla kaydedildi.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API'ye rezervasyon gönderilemedi. Status: {StatusCode}, Error: {Error}", response.StatusCode, errorContent);
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
                // API'den rezervasyonları getir
                var client = CreateHttpClient();
                var response = await client.GetAsync("api/reservation");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("API'den rezervasyonlar alınamadı. Status: {StatusCode}", response.StatusCode);
                    TempData["ErrorMessage"] = "Rezervasyonlar listelenirken bir hata oluştu.";
                    return RedirectToAction("Index");
                }

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var reservations = JsonSerializer.Deserialize<List<Reservation>>(json, options) ?? new List<Reservation>();

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
                // API'den rezervasyon sil
                var client = CreateHttpClient();
                var response = await client.DeleteAsync($"api/reservation/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Rezervasyon başarıyla silindi.";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    TempData["ErrorMessage"] = "Silinecek rezervasyon bulunamadı.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API'den rezervasyon silinemedi. Status: {StatusCode}, Error: {Error}", response.StatusCode, errorContent);
                    TempData["ErrorMessage"] = "Rezervasyon silinirken bir hata oluştu.";
                }
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Silinecek rezervasyon bulunamadı.";
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