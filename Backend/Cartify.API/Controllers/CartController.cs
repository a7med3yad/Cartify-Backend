using Cartify.Application.Contracts.CartDtos;
using Cartify.Application.Services.Interfaces.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cartify.API.Controllers
{
    [Route("api/controller/products/user")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("{userId:int}/cart/{productDetailId:int}")]
        public async Task<IActionResult> AddToCart([FromRoute] int userId, [FromRoute] int productDetailId, [FromBody] AddCartItemDto dto)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            if (productDetailId <= 0)
                return BadRequest(new { message = "Invalid product detail ID" });

            if (dto == null)
                return BadRequest(new { message = "Invalid body" });

            if (userId != dto.UserId || productDetailId != dto.ProductDetailId)
                return BadRequest(new { message = "Route and body IDs do not match" });

            try
            {
                await _cartService.AddToCartAsync(dto);
                return Ok(new { message = "Product added to cart successfully", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to add product to cart", detail = ex.Message });
            }
        }

        [HttpDelete("{userId:int}/cart/{productDetailId:int}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int userId, [FromRoute] int productDetailId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            if (productDetailId <= 0)
                return BadRequest(new { message = "Invalid product detail ID" });

            try
            {
                await _cartService.RemoveFromCartAsync(userId, productDetailId);
                return Ok(new { message = "Product removed from cart successfully", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to remove product from cart", detail = ex.Message });
            }
        }

        [HttpGet("{userId:int}/cart")]
        public async Task<IActionResult> GetCartByUserId([FromRoute] int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            try
            {
                var items = await _cartService.GetCartByUserIdAsync(userId);
                if (items == null)
                    return NotFound(new { message = "Cart not found" });

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to retrieve cart", detail = ex.Message });
            }
        }

        [HttpPut("{userId:int}/cart/quantity")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromRoute] int userId, [FromBody] UpdateCartItemQuantityDto dto)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            if (dto == null)
                return BadRequest(new { message = "Invalid body" });

            try
            {
                await _cartService.UpdateCartItemQuantityAsync(dto);
                return Ok(new { message = "Cart item quantity updated successfully", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to update cart item quantity", detail = ex.Message });
            }
        }
    }
}
