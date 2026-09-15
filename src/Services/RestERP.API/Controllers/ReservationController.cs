using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.Reservations.Commands.CreateReservation;
using RestERP.Application.Features.Reservations.Commands.DeleteReservation;
using RestERP.Application.Features.Reservations.Commands.UpdateReservation;
using RestERP.Application.Features.Reservations.Queries.GetReservationById;
using RestERP.Application.Features.Reservations.Queries.GetReservations;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : BaseApiController
    {
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(ILogger<ReservationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
        {
            try
            {
                var reservations = await Mediator.Send(new GetReservationsQuery());
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm rezervasyonlar getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservationById(int id)
        {
            try
            {
                var reservation = await Mediator.Send(new GetReservationByIdQuery(id));
                if (reservation == null)
                    return NotFound($"ID {id} olan rezervasyon bulunamadı");

                return Ok(reservation);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Rezervasyon bulunamadı: {ReservationId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon getirilirken hata oluştu: {ReservationId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdReservation = await Mediator.Send(new CreateReservationCommand(reservation));
                return CreatedAtAction(nameof(GetReservationById), new { id = createdReservation.Id }, createdReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            try
            {
                if (id != reservation.Id)
                    return BadRequest("ID uyumsuzluğu");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await Mediator.Send(new UpdateReservationCommand(reservation));
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek rezervasyon bulunamadı: {ReservationId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon güncellenirken hata oluştu: {ReservationId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                await Mediator.Send(new DeleteReservationCommand(id));
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Silinecek rezervasyon bulunamadı: {ReservationId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon silinirken hata oluştu: {ReservationId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }
    }
}
