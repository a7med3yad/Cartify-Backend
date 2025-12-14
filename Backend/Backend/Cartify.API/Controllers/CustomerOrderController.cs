using Cartify.Application.Contracts.OrderDtos;
using Cartify.Application.Services.Interfaces.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cartify.API.Controllers
{
    [Route("api/customer/orders")]
    [ApiController]
    [Authorize]
    public class CustomerOrderController : ControllerBase
    {
        private readonly ICustomerOrderService _orderService;

        public CustomerOrderController(ICustomerOrderService orderService)
        {
            _orderService = orderService;
        }

        // =========================================================
        // 🔹 CREATE ORDER
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            // Get customer ID from claims (you might need to adjust this based on your auth setup)
            var customerIdClaim = User.FindFirst("CustomerId")?.Value ?? User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
                return Unauthorized(new { message = "Invalid customer ID" });

            var order = await _orderService.CreateOrderAsync(customerId, dto);
            if (order == null)
                return BadRequest(new { message = "Failed to create order. Please check product availability and try again." });

            return Ok(new { message = "Order created successfully", order, success = true });
        }

        // =========================================================
        // 🔹 GET ORDER BY ID
        // =========================================================
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById([FromRoute] string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return BadRequest(new { message = "Order ID is required" });

            var customerIdClaim = User.FindFirst("CustomerId")?.Value ?? User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
                return Unauthorized(new { message = "Invalid customer ID" });

            var order = await _orderService.GetOrderByIdAsync(orderId, customerId);
            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }

        // =========================================================
        // 🔹 GET CUSTOMER ORDERS (Paged)
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetCustomerOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var customerIdClaim = User.FindFirst("CustomerId")?.Value ?? User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
                return Unauthorized(new { message = "Invalid customer ID" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var orders = await _orderService.GetCustomerOrdersAsync(customerId, page, pageSize);
            return Ok(orders);
        }

        // =========================================================
        // 🔹 CANCEL ORDER
        // =========================================================
        [HttpPut("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder([FromRoute] string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return BadRequest(new { message = "Order ID is required" });

            var customerIdClaim = User.FindFirst("CustomerId")?.Value ?? User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
                return Unauthorized(new { message = "Invalid customer ID" });

            var result = await _orderService.CancelOrderAsync(orderId, customerId);
            if (!result)
                return BadRequest(new { message = "Failed to cancel order. Order not found or cannot be cancelled." });

            return Ok(new { message = "Order cancelled successfully", success = true });
        }
    }
}

