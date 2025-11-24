using Cartify.Application.Contracts.WishlistDtos;
using Cartify.Application.Services.Interfaces.Product;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers
{
    [Route("api/controller/products/user")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishlistController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        // =========================================================
        // 🔹 ADD ITEM TO WISHLIST
        // =========================================================
        [HttpPost("{userId:int}/wishlist/{productId:int}")]
        public async Task<IActionResult> AddToWishList([FromRoute] int userId, [FromRoute] int productId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            if (productId <= 0)
                return BadRequest(new { message = "Invalid product ID" });

            try
            {
                await _wishListService.AddToWishListAsync(userId, productId);
                return Ok(new { message = "Product added to wishlist successfully", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to add product to wishlist", detail = ex.Message });
            }
        }

        // =========================================================
        // 🔹 REMOVE ITEM FROM WISHLIST
        // =========================================================
        [HttpDelete("{userId:int}/wishlist/{productId:int}")]
        public async Task<IActionResult> RemoveFromWishList([FromRoute] int userId, [FromRoute] int productId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            if (productId <= 0)
                return BadRequest(new { message = "Invalid product ID" });

            try
            {
                await _wishListService.RemoveFromWishListAsync(userId, productId);
                return Ok(new { message = "Product removed from wishlist successfully", success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to remove product from wishlist", detail = ex.Message });
            }
        }

        // =========================================================
        // 🔹 GET WISHLIST ITEMS FOR A USER
        // =========================================================
        [HttpGet("{userId:int}/wishlist")]
        public async Task<IActionResult> GetWishListByUserId([FromRoute] int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            try
            {
                var items = await _wishListService.GetWishListByUserIdAsync(userId);
                if (items == null)
                    return NotFound(new { message = "Wishlist not found" });

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to retrieve wishlist", detail = ex.Message });
            }
        }
    }
}
