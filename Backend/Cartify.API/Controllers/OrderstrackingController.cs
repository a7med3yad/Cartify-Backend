using Cartify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderstrackingController : ControllerBase
    {
        private readonly IOrdertrackingservice _orderService;

        public OrderstrackingController(IOrdertrackingservice orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(string orderId)
        {
            var order = await _orderService.GetOrderDetailsAsync(orderId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            var result = await _orderService.CancelOrderAsync(orderId);
            if (!result)
                return BadRequest("Unable to cancel order");

            return Ok("Order cancelled successfully");
        }
    }
}