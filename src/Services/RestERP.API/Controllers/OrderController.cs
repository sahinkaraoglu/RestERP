using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Enums;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Tüm siparişleri getirir
        /// </summary>
        /// <returns>Sipariş listesi</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm siparişler getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// ID'ye göre sipariş getirir
        /// </summary>
        /// <param name="id">Sipariş ID'si</param>
        /// <returns>Sipariş bilgisi</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound($"ID {id} olan sipariş bulunamadı");
                }
                return Ok(order);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Sipariş bulunamadı: {OrderId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş getirilirken hata oluştu: {OrderId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Detaylarıyla birlikte sipariş getirir
        /// </summary>
        /// <param name="orderId">Sipariş ID'si</param>
        /// <returns>Detaylı sipariş bilgisi</returns>
        [HttpGet("{orderId}/details")]
        public async Task<ActionResult<Order>> GetOrderWithDetails(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderWithDetailsAsync(orderId);
                if (order == null)
                {
                    return NotFound($"ID {orderId} olan sipariş bulunamadı");
                }
                return Ok(order);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Sipariş detayları bulunamadı: {OrderId}", orderId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş detayları getirilirken hata oluştu: {OrderId}", orderId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Masaya göre siparişleri getirir
        /// </summary>
        /// <param name="tableId">Masa ID'si</param>
        /// <returns>Masaya ait sipariş listesi</returns>
        [HttpGet("table/{tableId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByTable(int tableId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByTableIdAsync(tableId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masaya göre siparişler getirilirken hata oluştu: {TableId}", tableId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Aktif siparişleri getirir
        /// </summary>
        /// <returns>Aktif sipariş listesi</returns>
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Order>>> GetActiveOrders()
        {
            try
            {
                var orders = await _orderService.GetActiveOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aktif siparişler getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Tarihe göre siparişleri getirir
        /// </summary>
        /// <param name="date">Tarih</param>
        /// <returns>Tarihe ait sipariş listesi</returns>
        [HttpGet("date/{date}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDate(DateTime date)
        {
            try
            {
                var orders = await _orderService.GetOrdersByDateAsync(date);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tarihe göre siparişler getirilirken hata oluştu: {Date}", date);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Tarih aralığına göre siparişleri getirir
        /// </summary>
        /// <param name="startDate">Başlangıç tarihi</param>
        /// <param name="endDate">Bitiş tarihi</param>
        /// <returns>Tarih aralığına ait sipariş listesi</returns>
        [HttpGet("daterange")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tarih aralığına göre siparişler getirilirken hata oluştu: {StartDate} - {EndDate}", startDate, endDate);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Yeni sipariş oluşturur
        /// </summary>
        /// <param name="order">Sipariş bilgileri</param>
        /// <returns>Oluşturulan sipariş</returns>
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdOrder = await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Sipariş bilgilerini günceller
        /// </summary>
        /// <param name="id">Sipariş ID'si</param>
        /// <param name="order">Güncellenecek sipariş bilgileri</param>
        /// <returns>Güncellenme sonucu</returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            try
            {
                if (id != order.Id)
                {
                    return BadRequest("ID uyumsuzluğu");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _orderService.UpdateOrderAsync(order);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Güncellenecek sipariş bulunamadı: {OrderId}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş güncellenirken hata oluştu: {OrderId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Sipariş durumunu günceller
        /// </summary>
        /// <param name="orderId">Sipariş ID'si</param>
        /// <param name="status">Yeni durum</param>
        /// <returns>Güncellenme sonucu</returns>
        [HttpPut("{orderId}/status")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatus status)
        {
            try
            {
                var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
                if (!result)
                {
                    return NotFound($"ID {orderId} olan sipariş bulunamadı");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş durumu güncellenirken hata oluştu: {OrderId}", orderId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Sipariş siler
        /// </summary>
        /// <param name="id">Silinecek sipariş ID'si</param>
        /// <returns>Silme sonucu</returns>
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await _orderService.DeleteOrderAsync(id);
                if (!result)
                {
                    return NotFound($"ID {id} olan sipariş bulunamadı");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş silinirken hata oluştu: {OrderId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        /// <summary>
        /// Sipariş öğesini siler
        /// </summary>
        /// <param name="orderItemId">Silinecek sipariş öğesi ID'si</param>
        /// <returns>Silme sonucu</returns>
        [HttpDelete("item/{orderItemId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteOrderItem(int orderItemId)
        {
            try
            {
                var result = await _orderService.DeleteOrderItemAsync(orderItemId);
                if (!result)
                {
                    return NotFound($"ID {orderItemId} olan sipariş öğesi bulunamadı");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş öğesi silinirken hata oluştu: {OrderItemId}", orderItemId);
                return StatusCode(500, "Sunucu hatası");
            }
        }
    }
}
