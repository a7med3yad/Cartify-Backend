using Cartify.Application.Contracts.PromotionDtos;
using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/merchant/promotions")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class PromotionController : ControllerBase
    {
        private readonly IMerchantPromotionServices _promotionServices;

        public PromotionController(IMerchantPromotionServices promotionServices)
        {
            _promotionServices = promotionServices;
        }

        // =========================================================
        // 🔹 CREATE PROMOTION → POST /api/merchant/promotions
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> AddPromotion([FromBody] CreatePromotionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            if (dto.StartDate >= dto.EndDate)
                return BadRequest(new { message = "Start date must be before end date" });

            if (dto.DiscountPercentage < 0 || dto.DiscountPercentage > 100)
                return BadRequest(new { message = "Discount percentage must be between 0 and 100" });

            var result = await _promotionServices.AddPromotionAsync(dto);
            if (!result)
                return BadRequest(new { message = "Failed to create promotion. Please check your input and try again." });

            return Ok(new { message = "Promotion created successfully", success = true });
        }

        // =========================================================
        // 🔹 UPDATE PROMOTION → PUT /api/merchant/promotions/{promotionId}
        // =========================================================
        [HttpPut("{promotionId:int}")]
        public async Task<IActionResult> UpdatePromotion(
            [FromRoute] int promotionId,
            [FromBody] UpdatePromotionDto dto)
        {
            if (promotionId <= 0)
                return BadRequest(new { message = "Invalid promotion ID" });

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            if (dto.StartDate.HasValue && dto.EndDate.HasValue && dto.StartDate >= dto.EndDate)
                return BadRequest(new { message = "Start date must be before end date" });

            if (dto.DiscountPercentage.HasValue &&
                (dto.DiscountPercentage < 0 || dto.DiscountPercentage > 100))
            {
                return BadRequest(new { message = "Discount percentage must be between 0 and 100" });
            }

            var result = await _promotionServices.UpdatePromotionAsync(promotionId, dto);
            if (!result)
                return NotFound(new { message = "Promotion not found or failed to update" });

            return Ok(new { message = "Promotion updated successfully", success = true });
        }

        // =========================================================
        // 🔹 DELETE PROMOTION → DELETE /api/merchant/promotions/{promotionId}
        // =========================================================
        [HttpDelete("{promotionId:int}")]
        public async Task<IActionResult> DeletePromotion([FromRoute] int promotionId)
        {
            if (promotionId <= 0)
                return BadRequest(new { message = "Invalid promotion ID" });

            var result = await _promotionServices.DeletePromotionAsync(promotionId);
            if (!result)
                return NotFound(new { message = "Promotion not found" });

            return Ok(new { message = "Promotion deleted successfully", success = true });
        }

        // =========================================================
        // 🔹 GET ALL PROMOTIONS
        //     → GET /api/merchant/promotions
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetAllPromotions()
        {
            var promotions = await _promotionServices.GetAllPromotionsAsync();
            return Ok(promotions);
        }

        // =========================================================
        // 🔹 GET PROMOTION BY ID
        //     → GET /api/merchant/promotions/{promotionId}
        // =========================================================
        [HttpGet("{promotionId:int}")]
        public async Task<IActionResult> GetPromotionById([FromRoute] int promotionId)
        {
            if (promotionId <= 0)
                return BadRequest(new { message = "Invalid promotion ID" });

            var promotions = await _promotionServices.GetAllPromotionsAsync();
            var promotion = promotions.FirstOrDefault(p => p.PromotionId == promotionId);

            if (promotion == null)
                return NotFound(new { message = "Promotion not found" });

            return Ok(promotion);
        }

        // =========================================================
        // 🔹 GET PROMOTION BY PRODUCT DETAIL ID
        //     → GET /api/merchant/promotions/by-product-detail/{productDetailId}
        // =========================================================
        [HttpGet("by-product-detail/{productDetailId:int}")]
        public async Task<IActionResult> GetPromotionByProductDetailId([FromRoute] int productDetailId)
        {
            if (productDetailId <= 0)
                return BadRequest(new { message = "Invalid product detail ID" });

            var promotion = await _promotionServices.GetPromotionByProductDetailIdAsync(productDetailId);
            if (promotion == null)
                return NotFound(new { message = "No promotion found for this product detail" });

            return Ok(promotion);
        }

        // =========================================================
        // 🔹 GET PROMOTIONS BY SUBCATEGORY ID
        //     → GET /api/merchant/promotions/by-subcategory/{subCategoryId}
        // =========================================================
        [HttpGet("by-subcategory/{subCategoryId:int}")]
        public async Task<IActionResult> GetPromotionsBySubCategoryId([FromRoute] int subCategoryId)
        {
            if (subCategoryId <= 0)
                return BadRequest(new { message = "Invalid subcategory ID" });

            var promotions = await _promotionServices.GetPromotionsBySubCategoryIdAsync(subCategoryId);
            if (!promotions.Any())
                return NotFound(new { message = "No promotions found for this subcategory" });

            return Ok(promotions);
        }
    }
}
