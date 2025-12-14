using System.Threading.Tasks;
using Cartify.Application.Contracts.OrderDtos;
using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/merchant/orders")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class OrderController : ControllerBase
    {
        private readonly IMerchantOrderServices _orderServices;

        public OrderController(IMerchantOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        // =========================================================
        // 🔹 GET ORDER BY ID
        // =========================================================
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById([FromRoute] string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return BadRequest(new { message = "Order ID is required" });

            var order = await _orderServices.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }

        // =========================================================
        // 🔹 GET ORDERS BY STORE ID (Paged)
        // =========================================================
        [HttpGet("store/{storeId:int}")]
        public async Task<IActionResult> GetOrdersByStoreId(
            [FromRoute] int storeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (storeId <= 0)
                return BadRequest(new { message = "Invalid store ID" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var orders = await _orderServices.GetOrdersByStoreIdAsync(storeId, page, pageSize);
            return Ok(orders);
        }

        // =========================================================
        // 🔹 UPDATE ORDER STATUS
        // =========================================================
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(
            [FromRoute] string orderId,
            [FromBody] UpdateOrderStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return BadRequest(new { message = "Order ID is required" });

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            var result = await _orderServices.UpdateOrderStatusAsync(orderId, dto.Status);

            if (!result)
                return BadRequest(new { message = "Failed to update order status. Order not found or invalid status." });

            return Ok(new { message = $"Order status updated to '{dto.Status}' successfully", success = true });
        }

        // =========================================================
        // 🔹 FILTER ORDERS
        // =========================================================
        [HttpGet("filter")]
        public async Task<IActionResult> FilterOrders(
            [FromQuery] int storeId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (storeId <= 0)
                return BadRequest(new { message = "Invalid store ID" });

            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                return BadRequest(new { message = "Start date cannot be after end date" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            try
            {
                var result = await _orderServices.FilterOrdersAsync(storeId, startDate, endDate, status, page, pageSize);
                return Ok(result);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { message = "Filter orders feature is not yet implemented" });
            }
        }
    }
}
