using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestERP.Web.Models;
using RestERP.Application.Services.Abstract;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Core.Domain.Entities;
using System.Text;
using System.Text.Json;

namespace RestERP.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class TableController : Controller
{
    private readonly ILogger<TableController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public TableController(ILogger<TableController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync("api/table");
            
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Masa listesi alınırken bir hata oluştu.";
                return View(new List<Table>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var tables = JsonSerializer.Deserialize<List<Table>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(tables ?? new List<Table>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa listesi alınırken hata oluştu");
            TempData["ErrorMessage"] = "Masa listesi alınırken bir hata oluştu: " + ex.Message;
            return View("Error");
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Table table)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(table);
            }

            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var json = JsonSerializer.Serialize(table);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/table", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Masa başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Masa oluşturulurken bir hata oluştu.");
                return View(table);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa oluşturulurken hata oluştu");
            ModelState.AddModelError("", "Masa oluşturulurken bir hata oluştu: " + ex.Message);
            return View(table);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync($"api/table/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Masa bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var json = await response.Content.ReadAsStringAsync();
            var table = JsonSerializer.Deserialize<Table>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (table == null)
            {
                TempData["ErrorMessage"] = "Masa bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(table);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa düzenleme sayfası açılırken hata oluştu. Id: {Id}", id);
            TempData["ErrorMessage"] = "Masa düzenleme sayfası açılırken bir hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Table table)
    {
        try
        {
            if (id != table.Id)
            {
                TempData["ErrorMessage"] = "ID uyuşmazlığı.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(table);
            }

            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var json = JsonSerializer.Serialize(table);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"api/table/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Masa başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Masa güncellenirken bir hata oluştu.");
                return View(table);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa güncellenirken hata oluştu. Id: {Id}", id);
            ModelState.AddModelError("", "Masa güncellenirken bir hata oluştu: " + ex.Message);
            return View(table);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.GetAsync($"api/table/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Masa bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var json = await response.Content.ReadAsStringAsync();
            var table = JsonSerializer.Deserialize<Table>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (table == null)
            {
                TempData["ErrorMessage"] = "Masa bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(table);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa silme sayfası açılırken hata oluştu. Id: {Id}", id);
            TempData["ErrorMessage"] = "Masa silme sayfası açılırken bir hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var response = await httpClient.DeleteAsync($"api/table/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Masa başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Masa silinirken bir hata oluştu.";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa silinirken hata oluştu. Id: {Id}", id);
            TempData["ErrorMessage"] = "Masa silinirken bir hata oluştu: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> SetTableStatus(int id, bool isOccupied)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RestERPApi");
            var json = JsonSerializer.Serialize(isOccupied);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"api/table/{id}/status", content);
            
            if (response.IsSuccessStatusCode)
            {
                var status = isOccupied ? "dolu" : "boş";
                return Json(new { success = true, message = $"Masa durumu {status} olarak güncellendi." });
            }
            else
            {
                return Json(new { success = false, message = "Masa durumu güncellenirken bir hata oluştu." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa durumu güncellenirken hata oluştu. Id: {Id}", id);
            return Json(new { success = false, message = "Masa durumu güncellenirken bir hata oluştu: " + ex.Message });
        }
    }

    public IActionResult IsPaid()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
