using Cartify.Application.Contracts.CategoryDtos;
using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMerchantCategoryServices _categoryServices;

        public CategoryController(
            IMerchantCategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        // =========================================================
        // 🔹 CREATE CATEGORY
        // =========================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryServices.CreateCategoryAsync(dto);
            if (!result)
                return BadRequest("Failed to create category (already exists or invalid data).");

            return Ok(new { message = "Category created successfully ✅" });
        }

        // =========================================================
        // 🔹 UPDATE CATEGORY
        // =========================================================
        [HttpPut("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory([FromRoute] int categoryId, [FromForm] CreateCategoryDto dto)
        {
            var result = await _categoryServices.UpdateCategoryAsync(categoryId, dto);
            if (!result)
                return NotFound(new { message = "Category not found or update failed ❌" });

            return Ok(new { message = "Category updated successfully ✅" });
        }

        // =========================================================
        // 🔹 DELETE CATEGORY
        // =========================================================
        [HttpDelete("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            var result = await _categoryServices.DeleteCategoryAsync(categoryId);
            if (!result)
                return NotFound(new { message = "Category not found or already deleted ❌" });

            return Ok(new { message = "Category deleted successfully ✅" });
        }

        // =========================================================
        // 🔹 GET ALL CATEGORIES
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _categoryServices.GetAllCategoriesAsync(page, pageSize);
            return Ok(result);
        }

        // =========================================================
        // 🔹 GET CATEGORY BY ID
        // =========================================================
        [HttpGet("{categoryId:int}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById([FromRoute] int categoryId)
        {
            var category = await _categoryServices.GetCategoryByIdAsync(categoryId);
            if (category == null)
                return NotFound(new { message = "Category not found ❌" });

            return Ok(category);
        }

        // =========================================================
        // 🔹 GET PRODUCT COUNT BY CATEGORY
        // =========================================================
        [HttpGet("{categoryId:int}/products/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductCountByCategoryId([FromRoute] int categoryId)
        {
            var count = await _categoryServices.GetProductCountByCategoryIdAsync(categoryId);
            return Ok(new { categoryId, productCount = count });
        }

        // =========================================================
        // 🔹 GET PRODUCTS BY CATEGORY
        // =========================================================
        [HttpGet("{categoryId:int}/products")]
        public async Task<IActionResult> GetProductsByCategory([FromRoute] int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _categoryServices.GetProductsByCategoryIdAsync(categoryId, page, pageSize);
            return Ok(products);
        }

        // =========================================================
        // 🔹 GET PRODUCTS BY SUBCATEGORY
        // =========================================================
        [HttpGet("subcategory/{subCategoryId:int}/products")]
        public async Task<IActionResult> GetProductsBySubCategory([FromRoute] int subCategoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _categoryServices.GetProductsBySubCategoryIdAsync(subCategoryId, page, pageSize);
            return Ok(products);
        }
        // =========================================================
        // 🔹 CREATE SUBCATEGORY
        // =========================================================
        [HttpPost("subcategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSubCategory([FromForm] CreateSubCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _categoryServices.CreateSubCategoryAsync(dto);
            if (!result)
                return BadRequest("Failed to create subcategory (already exists or invalid data).");

            return Ok(new { message = "Subcategory created successfully ✅" });
        }

        // =========================================================
        // 🔹 UPDATE SUBCATEGORY
        // =========================================================
        [HttpPut("subcategory/{subCategoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSubCategory([FromRoute] int subCategoryId, [FromForm] CreateSubCategoryDto dto)
        {
            var result = await _categoryServices.UpdateSubCategoryAsync(subCategoryId, dto);
            if (!result)
                return NotFound(new { message = "Subcategory not found or update failed ❌" });

            return Ok(new { message = "Subcategory updated successfully ✅" });
        }

        // =========================================================
        // 🔹 DELETE SUBCATEGORY
        // =========================================================
        [HttpDelete("subcategory/{subCategoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSubCategory([FromRoute] int subCategoryId)
        {
            var result = await _categoryServices.DeleteSubCategoryAsync(subCategoryId);
            if (!result)
                return NotFound(new { message = "Subcategory not found or already deleted ❌" });

            return Ok(new { message = "Subcategory deleted successfully ✅" });
        }

        // =========================================================
        // 🔹 GET ALL SUBCATEGORIES
        // =========================================================
        [HttpGet("subcategory")]
        [ProducesResponseType(typeof(IEnumerable<SubCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSubCategories()
        {
            var result = await _categoryServices.GetAllSubCategoriesAsync();
            return Ok(result ?? new List<SubCategoryDto>());
        }

        // =========================================================
        // 🔹 GET SUBCATEGORY BY ID
        // =========================================================
        [HttpGet("subcategory/{subCategoryId:int}")]
        [ProducesResponseType(typeof(SubCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSubCategoryById([FromRoute] int subCategoryId)
        {
            var subCategory = await _categoryServices.GetSubCategoryByIdAsync(subCategoryId);
            if (subCategory == null)
                return NotFound(new { message = "Subcategory not found ❌" });

            return Ok(subCategory);
        }
    }
}
