using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestERP.Web.Models;
using RestERP.Application.Services.Interfaces;
using RestERP.Infrastructure.Data.SeedData;
using RestERP.Core.Doman.Entities;

namespace RestERP.Web.Controllers;

public class TableController : Controller
{
    private readonly ILogger<TableController> _logger;
    private readonly ITableService _tableService;

    public TableController(ILogger<TableController> logger, ITableService tableService)
    {
        _logger = logger;
        _tableService = tableService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var tables = await _tableService.GetAllTablesAsync();
            return View("~/Views/Panel/Table/Index.cshtml", tables);
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

            await _tableService.CreateTableAsync(table);
            TempData["SuccessMessage"] = "Masa başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
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
            var table = await _tableService.GetTableByIdAsync(id);
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

            await _tableService.UpdateTableAsync(table);
            TempData["SuccessMessage"] = "Masa başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
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
            var table = await _tableService.GetTableByIdAsync(id);
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
            await _tableService.DeleteTableAsync(id);
            TempData["SuccessMessage"] = "Masa başarıyla silindi.";
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
            await _tableService.SetTableOccupiedStatusAsync(id, isOccupied);
            var status = isOccupied ? "dolu" : "boş";
            return Json(new { success = true, message = $"Masa durumu {status} olarak güncellendi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Masa durumu güncellenirken hata oluştu. Id: {Id}", id);
            return Json(new { success = false, message = "Masa durumu güncellenirken bir hata oluştu: " + ex.Message });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
