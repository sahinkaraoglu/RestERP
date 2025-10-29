using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm rezervasyonları getirir
        /// </summary>
        /// <returns>Rezervasyon listesi</returns>
        [HttpGet]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
        {
            try
            {
                var reservations = await _reservationService.GetAllReservationsAsync();
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm rezervasyonlar getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// ID'ye göre rezervasyon getirir
        /// </summary>
        /// <param name="id">Rezervasyon ID'si</param>
        /// <returns>Rezervasyon bilgisi</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservationById(int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    return NotFound($"ID {id} olan rezervasyon bulunamadı");
                }
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

        /// <summary>
        /// Yeni rezervasyon oluşturur
        /// </summary>
        /// <param name="reservation">Rezervasyon bilgileri</param>
        /// <returns>Oluşturulan rezervasyon</returns>
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdReservation = await _reservationService.CreateReservationAsync(reservation);
                return CreatedAtAction(nameof(GetReservationById), new { id = createdReservation.Id }, createdReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rezervasyon oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Rezervasyon bilgilerini günceller
        /// </summary>
        /// <param name="id">Rezervasyon ID'si</param>
        /// <param name="reservation">Güncellenecek rezervasyon bilgileri</param>
        /// <returns>Güncellenme sonucu</returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            try
            {
                if (id != reservation.Id)
                {
                    return BadRequest("ID uyumsuzluğu");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _reservationService.UpdateReservationAsync(reservation);
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

        /// <summary>
        /// Rezervasyon siler
        /// </summary>
        /// <param name="id">Silinecek rezervasyon ID'si</param>
        /// <returns>Silme sonucu</returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                await _reservationService.DeleteReservationAsync(id);
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
