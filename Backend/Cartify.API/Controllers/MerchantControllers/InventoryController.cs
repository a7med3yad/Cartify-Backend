using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/merchant/inventory")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class InventoryController : ControllerBase
    {
        private readonly IMerchantInventoryServices _inventoryServices;
        public InventoryController(IMerchantInventoryServices _inventoryServices)
        {
            this._inventoryServices = _inventoryServices;
        }

        // =========================================================
        // 🔹 GET INVENTORY BY STORE ID (Paged)
        // =========================================================
        [HttpGet("store/{storeId:int}")]
        public async Task<IActionResult> GetInventoryByStoreId(
            [FromRoute] int storeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (storeId <= 0)
                return BadRequest(new { message = "Invalid store ID" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var inventory = await _inventoryServices.GetInventoryByStoreIdAsync(storeId, page, pageSize);
            return Ok(inventory);
        }

        // =========================================================
        // 🔹 GET INVENTORY BY PRODUCT DETAIL ID
        // =========================================================
        [HttpGet("product-detail/{productDetailId:int}")]
        public async Task<IActionResult> GetInventoryByProductDetailId([FromRoute] int productDetailId)
        {
            if (productDetailId <= 0)
                return BadRequest(new { message = "Invalid product detail ID" });

            var inventory = await _inventoryServices.GetInventoryByProductDetailIdAsync(productDetailId);
            if (inventory == null)
                return NotFound(new { message = "Inventory not found for this product detail" });

            return Ok(inventory);
        }

        // =========================================================
        // 🔹 UPDATE STOCK QUANTITY
        // =========================================================
        [HttpPut("product-detail/{productDetailId:int}/stock")]
        public async Task<IActionResult> UpdateStockQuantity(
            [FromRoute] int productDetailId,
            [FromBody] UpdateStockDto dto)
        {
            if (productDetailId <= 0)
                return BadRequest(new { message = "Invalid product detail ID" });

            if (dto.NewQuantity < 0)
                return BadRequest(new { message = "Quantity cannot be negative" });

            var result = await _inventoryServices.UpdateStockQuantityAsync(productDetailId, dto.NewQuantity);
            if (!result)
                return BadRequest(new { message = "Failed to update stock quantity. Product detail not found." });

            return Ok(new { message = "Stock quantity updated successfully", success = true });
        }

        // =========================================================
        // 🔹 GET LOW STOCK ALERTS
        // =========================================================
        [HttpGet("store/{storeId:int}/low-stock")]
        public async Task<IActionResult> GetLowStockAlerts(
            [FromRoute] int storeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (storeId <= 0)
                return BadRequest(new { message = "Invalid store ID" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var alerts = await _inventoryServices.GetLowStockAlertsAsync(storeId, page, pageSize);
            return Ok(alerts);
        }
    }

    public class UpdateStockDto
    {
        public int NewQuantity { get; set; }
    }
}
