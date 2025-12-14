using Cartify.Application.Contracts.ProductDtos;
using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/merchant/products")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class ProductController : ControllerBase
    {
        private readonly IMerchantProductServices _productServices;

        public ProductController(IMerchantProductServices productServices)
        {
            _productServices = productServices;
        }

        // =========================================================
        // 🔹 CREATE PRODUCT
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            var result = await _productServices.AddProductAsync(dto);
            if (!result)
                return BadRequest(new { message = "Failed to create product. Please check your input and try again." });

            return Ok(new { message = "Product created successfully", success = true });
        }

        // =========================================================
        // 🔹 UPDATE PRODUCT
        // =========================================================
        [HttpPut("{productId:int}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromForm] UpdateProductDto dto)
        {
            if (productId <= 0)
                return BadRequest(new { message = "Invalid product ID" });

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            var result = await _productServices.UpdateProductAsync(productId, dto);
            if (!result)
                return NotFound(new { message = "Product not found or failed to update" });

            return Ok(new { message = "Product updated successfully", success = true });
        }

        // =========================================================
        // 🔹 DELETE PRODUCT
        // =========================================================
        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
        {
            if (productId <= 0)
                return BadRequest(new { message = "Invalid product ID" });

            var result = await _productServices.DeleteProductAsync(productId);
            if (!result)
                return NotFound(new { message = "Product not found" });

            return Ok(new { message = "Product deleted successfully", success = true });
        }

        // =========================================================
        // 🔹 ADD PRODUCT IMAGES
        // =========================================================
        [HttpPost("{productId:int}/images")]
        public async Task<IActionResult> AddProductImages([FromRoute] int productId, [FromForm] List<IFormFile> images)
        {
            if (productId <= 0)
                return BadRequest(new { message = "Invalid product ID" });

            if (images == null || images.Count == 0)
                return BadRequest(new { message = "No images provided" });

            var result = await _productServices.AddProductImagesAsync(productId, images);
            if (!result)
                return BadRequest(new { message = "Failed to upload images. Please check product ID and image format." });

            return Ok(new { message = "Images uploaded successfully", success = true });
        }

        // =========================================================
        // 🔹 GET PRODUCT BY ID
        // =========================================================
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProductById([FromRoute] int productId)
        {
            if (productId <= 0)
                return BadRequest(new { message = "Invalid product ID" });

            var product = await _productServices.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        // =========================================================
        // 🔹 GET PRODUCTS BY MERCHANT (Paged)
        // =========================================================
        [HttpGet("merchant/{merchantId}")]
        public async Task<IActionResult> GetAllProductsByMerchantId(
            [FromRoute] string merchantId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
                return BadRequest(new { message = "Merchant ID is required" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var products = await _productServices.GetAllProductsByMerchantIdAsync(merchantId, page, pageSize);
            return Ok(products);
        }

        // =========================================================
        // 🔹 SEARCH PRODUCTS BY NAME
        // =========================================================
        [HttpGet("search")]
        public async Task<IActionResult> GetProductsByName(
            [FromQuery] string name,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search name is required" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var products = await _productServices.GetProductsByNameAsync(name, page, pageSize);
            return Ok(products);
        }

        // =========================================================
        // 🔹 GET PRODUCTS BY TYPE ID
        // =========================================================
        [HttpGet("type/{typeId:int}")]
        public async Task<IActionResult> GetProductsByTypeId(
            [FromRoute] int typeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (typeId <= 0)
                return BadRequest(new { message = "Invalid type ID" });

            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var products = await _productServices.GetProductsByTypeIdAsync(typeId, page, pageSize);
            return Ok(products);
        }

        // =========================================================
        // 🔹 GET PRODUCT DETAIL BY ID
        // =========================================================
        [HttpGet("details/{productDetailId:int}")]
        public async Task<IActionResult> GetProductDetail([FromRoute] int productDetailId)
        {
            if (productDetailId <= 0)
                return BadRequest(new { message = "Invalid product detail ID" });

            var productDetail = await _productServices.GetProductDetailAsync(productDetailId);
            if (productDetail == null)
                return NotFound(new { message = "Product detail not found" });

            return Ok(productDetail);
        }

        // =========================================================
        // 🔹 ADD PRODUCT DETAIL
        // =========================================================
        [HttpPost("details")]
        public async Task<IActionResult> AddProductDetail([FromBody] CreateProductDetailDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            var result = await _productServices.AddProductDetailAsync(dto);
            if (!result)
                return BadRequest(new { message = "Failed to create product detail. Please check your input." });

            return Ok(new { message = "Product detail created successfully", success = true });
        }

        // =========================================================
        // 🔹 UPDATE PRODUCT DETAIL
        // =========================================================
        [HttpPut("details")]
        public async Task<IActionResult> UpdateProductDetail([FromBody] UpdateProductDetailDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            var result = await _productServices.UpdateProductDetailAsync(dto);
            if (!result)
                return NotFound(new { message = "Product detail not found or failed to update" });

            return Ok(new { message = "Product detail updated successfully", success = true });
        }

        // =========================================================
        // 🔹 DELETE PRODUCT DETAIL
        // =========================================================
        [HttpDelete("details/{productDetailId:int}")]
        public async Task<IActionResult> DeleteProductDetail([FromRoute] int productDetailId)
        {
            if (productDetailId <= 0)
                return BadRequest(new { message = "Invalid product detail ID" });

            var result = await _productServices.DeleteProductDetailAsync(productDetailId);
            if (!result)
                return NotFound(new { message = "Product detail not found" });

            return Ok(new { message = "Product detail deleted successfully", success = true });
        }
    }
}
