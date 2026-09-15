using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestERP.Application.Features.Orders.Commands.CreateOrder;
using RestERP.Application.Features.Orders.Commands.DeleteOrder;
using RestERP.Application.Features.Orders.Commands.DeleteOrderItem;
using RestERP.Application.Features.Orders.Commands.UpdateOrder;
using RestERP.Application.Features.Orders.Commands.UpdateOrderStatus;
using RestERP.Application.Features.Orders.Queries.GetActiveOrders;
using RestERP.Application.Features.Orders.Queries.GetOrderById;
using RestERP.Application.Features.Orders.Queries.GetOrders;
using RestERP.Application.Features.Orders.Queries.GetOrdersByDate;
using RestERP.Application.Features.Orders.Queries.GetOrdersByDateRange;
using RestERP.Application.Features.Orders.Queries.GetOrdersByTableId;
using RestERP.Application.Features.Orders.Queries.GetOrderWithDetails;
using RestERP.Core.Domain.Entities;
using RestERP.Domain.Enums;
using RestERP.Domain.Exceptions;

namespace RestERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : BaseApiController
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            try
            {
                var orders = await Mediator.Send(new GetOrdersQuery());
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm siparişler getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                var order = await Mediator.Send(new GetOrderByIdQuery(id));
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Sipariş bulunamadı: {OrderId}", id);
                return NotFound(ex.Message);
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

        [HttpGet("{orderId}/details")]
        public async Task<ActionResult<Order>> GetOrderWithDetails(int orderId)
        {
            try
            {
                var order = await Mediator.Send(new GetOrderWithDetailsQuery(orderId));
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Sipariş detayları bulunamadı: {OrderId}", orderId);
                return NotFound(ex.Message);
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

        [HttpGet("table/{tableId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByTable(int tableId)
        {
            try
            {
                var orders = await Mediator.Send(new GetOrdersByTableIdQuery(tableId));
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masaya göre siparişler getirilirken hata oluştu: {TableId}", tableId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Order>>> GetActiveOrders()
        {
            try
            {
                var orders = await Mediator.Send(new GetActiveOrdersQuery());
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aktif siparişler getirilirken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("date/{date}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDate(DateTime date)
        {
            try
            {
                var orders = await Mediator.Send(new GetOrdersByDateQuery(date));
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tarihe göre siparişler getirilirken hata oluştu: {Date}", date);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpGet("daterange")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var orders = await Mediator.Send(new GetOrdersByDateRangeQuery(startDate, endDate));
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tarih aralığına göre siparişler getirilirken hata oluştu: {StartDate} - {EndDate}", startDate, endDate);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdOrder = await Mediator.Send(new CreateOrderCommand(order));
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş oluşturulurken hata oluştu");
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            try
            {
                if (id != order.Id)
                    return BadRequest("ID uyumsuzluğu");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await Mediator.Send(new UpdateOrderCommand(order));
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

        [HttpPut("{orderId}/status")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatus status)
        {
            try
            {
                var result = await Mediator.Send(new UpdateOrderStatusCommand(orderId, status));
                if (!result)
                    return NotFound($"ID {orderId} olan sipariş bulunamadı");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş durumu güncellenirken hata oluştu: {OrderId}", orderId);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var result = await Mediator.Send(new DeleteOrderCommand(id));
                if (!result)
                    return NotFound($"ID {id} olan sipariş bulunamadı");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sipariş silinirken hata oluştu: {OrderId}", id);
                return StatusCode(500, "Sunucu hatası");
            }
        }

        [HttpDelete("item/{orderItemId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteOrderItem(int orderItemId)
        {
            try
            {
                var result = await Mediator.Send(new DeleteOrderItemCommand(orderItemId));
                if (!result)
                    return NotFound($"ID {orderItemId} olan sipariş öğesi bulunamadı");

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
