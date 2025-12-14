using Cartify.Application.Contracts.OrderDtos;
using Cartify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCheckout([FromBody] CheckoutDto checkoutData)
        {
            try
            {
                var order = await _checkoutService.ProcessCheckoutAsync(checkoutData);
                return Ok(new
                {
                    success = true,
                    orderId = order.OrderId,
                    message = "Order placed successfully!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Checkout failed: " + ex.Message
                });
            }
        }
    }
}