using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.Tables.Commands.CreateTable;
using RestERP.Application.Features.Tables.Commands.DeleteTable;
using RestERP.Application.Features.Tables.Commands.SetTableOccupiedStatus;
using RestERP.Application.Features.Tables.Commands.UpdateTable;
using RestERP.Application.Features.Tables.Queries.GetTableById;
using RestERP.Application.Features.Tables.Queries.GetTables;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : BaseApiController
    {
        private readonly ILogger<TableController> _logger;

        public TableController(ILogger<TableController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Table>>> GetAllTables()
        {
            try
            {
                var tables = await Mediator.Send(new GetTablesQuery());
                return Ok(tables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm masalar getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Table>> GetTableById(int id)
        {
            try
            {
                var table = await Mediator.Send(new GetTableByIdQuery(id));
                return Ok(table);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Masa bulunamadı: {TableId}", id);
                return NotFound(ex.Message);
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Table>> CreateTable([FromBody] Table table)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdTable = await Mediator.Send(new CreateTableCommand(table));
                return CreatedAtAction(nameof(GetTableById), new { id = createdTable.Id }, createdTable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateTable(int id, [FromBody] Table table)
        {
            try
            {
                if (id != table.Id)
                    return BadRequest("ID uyumsuzluğu");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await Mediator.Send(new UpdateTableCommand(table));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek masa bulunamadı: {TableId}", id);
                return NotFound(ex.Message);
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

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteTable(int id)
        {
            try
            {
                await Mediator.Send(new DeleteTableCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Silinecek masa bulunamadı: {TableId}", id);
                return NotFound(ex.Message);
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

        [HttpPut("{id}/status")]
        [AllowAnonymous]
        public async Task<IActionResult> SetTableOccupiedStatus(int id, [FromBody] bool isOccupied)
        {
            try
            {
                await Mediator.Send(new SetTableOccupiedStatusCommand(id, isOccupied));
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"ID {id} olan masa bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa durumu güncellenirken hata oluştu: {TableId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }
    }
}
