using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        private readonly ILogger<TableController> _logger;

        public TableController(ITableService tableService, ILogger<TableController> logger)
        {
            _tableService = tableService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm masaları getirir
        /// </summary>
        /// <returns>Masa listesi</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Table>>> GetAllTables()
        {
            try
            {
                var tables = await _tableService.GetAllTablesAsync();
                return Ok(tables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm masalar getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// ID'ye göre masa getirir
        /// </summary>
        /// <param name="id">Masa ID'si</param>
        /// <returns>Masa bilgisi</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Table>> GetTableById(int id)
        {
            try
            {
                var table = await _tableService.GetTableByIdAsync(id);
                if (table == null)
                {
                    return NotFound($"ID {id} olan masa bulunamadı");
                }
                return Ok(table);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Masa bulunamadı: {TableId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa getirilirken hata oluştu: {TableId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Yeni masa oluşturur
        /// </summary>
        /// <param name="table">Masa bilgileri</param>
        /// <returns>Oluşturulan masa</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Table>> CreateTable([FromBody] Table table)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdTable = await _tableService.CreateTableAsync(table);
                return CreatedAtAction(nameof(GetTableById), new { id = createdTable.Id }, createdTable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Masa bilgilerini günceller
        /// </summary>
        /// <param name="id">Masa ID'si</param>
        /// <param name="table">Güncellenecek masa bilgileri</param>
        /// <returns>Güncellenme sonucu</returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateTable(int id, [FromBody] Table table)
        {
            try
            {
                if (id != table.Id)
                {
                    return BadRequest("ID uyumsuzluğu");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _tableService.UpdateTableAsync(table);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek masa bulunamadı: {TableId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa güncellenirken hata oluştu: {TableId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Masa siler
        /// </summary>
        /// <param name="id">Silinecek masa ID'si</param>
        /// <returns>Silme sonucu</returns>
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteTable(int id)
        {
            try
            {
                await _tableService.DeleteTableAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Silinecek masa bulunamadı: {TableId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa silinirken hata oluştu: {TableId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Masa doluluk durumunu günceller
        /// </summary>
        /// <param name="id">Masa ID'si</param>
        /// <param name="isOccupied">Dolu mu?</param>
        /// <returns>Güncellenme sonucu</returns>
        [HttpPut("{id}/status")]
        [AllowAnonymous]
        public async Task<IActionResult> SetTableOccupiedStatus(int id, [FromBody] bool isOccupied)
        {
            try
            {
                var result = await _tableService.SetTableOccupiedStatusAsync(id, isOccupied);
                if (!result)
                {
                    return NotFound($"ID {id} olan masa bulunamadı");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa durumu güncellenirken hata oluştu: {TableId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }
    }
}
