using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/merchant/attributes-measures")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class AttributeMeasureController : ControllerBase
    {
        private readonly IMerchantAttributeMeasureServices _attributeMeasureServices;

        public AttributeMeasureController(IMerchantAttributeMeasureServices attributeMeasureServices)
        {
            _attributeMeasureServices = attributeMeasureServices;
        }

        // =========================================================
        // 🔹 ATTRIBUTE METHODS
        // =========================================================

        /// <summary>
        /// Gets all attributes
        /// </summary>
        [HttpGet("attributes")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttributes()
        {
            var attributes = await _attributeMeasureServices.GetAttributesAsync();
            return Ok(attributes);
        }

        /// <summary>
        /// Checks if an attribute exists by name
        /// </summary>
        [HttpGet("attributes/check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAttribute([FromQuery][Required] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Attribute name is required" });

            var exists = await _attributeMeasureServices.GetAttributeAsync(name);
            return Ok(new { name, exists });
        }

        /// <summary>
        /// Adds a new attribute
        /// </summary>
        [HttpPost("attributes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAttribute([FromBody] AttributeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Attribute name is required" });

            var result = await _attributeMeasureServices.AddAttributeAsync(request.Name);
            if (!result)
                return BadRequest(new { message = "Failed to add attribute. It may already exist or the name is invalid." });

            return Ok(new { message = "Attribute added successfully ✅", name = request.Name });
        }

        // =========================================================
        // 🔹 MEASURE METHODS
        // =========================================================

        /// <summary>
        /// Gets all measures
        /// </summary>
        [HttpGet("measures")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeasures()
        {
            var measures = await _attributeMeasureServices.GetMeasuresAsync();
            return Ok(measures);
        }

        /// <summary>
        /// Checks if a measure exists by name
        /// </summary>
        [HttpGet("measures/check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMeasure([FromQuery][Required] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Measure name is required" });

            var exists = await _attributeMeasureServices.GetMeasureAsync(name);
            return Ok(new { name, exists });
        }

        /// <summary>
        /// Adds a new measure
        /// </summary>
        [HttpPost("measures")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMeasure([FromBody] MeasureRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Measure name is required" });

            var result = await _attributeMeasureServices.AddMeasureAsync(request.Name);
            if (!result)
                return BadRequest(new { message = "Failed to add measure. It may already exist or the name is invalid." });

            return Ok(new { message = "Measure added successfully ✅", name = request.Name });
        }

        // =========================================================
        // 🔹 MEASURE BY ATTRIBUTE METHODS
        // =========================================================

        /// <summary>
        /// Gets all measures associated with a specific attribute
        /// </summary>
        [HttpGet("attributes/{attributeName}/measures")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMeasuresByAttribute([FromRoute] string attributeName)
        {
            if (string.IsNullOrWhiteSpace(attributeName))
                return BadRequest(new { message = "Attribute name is required" });

            var measures = await _attributeMeasureServices.GetMeasuresByAttributeAsync(attributeName);
            return Ok(measures);
        }

        /// <summary>
        /// Adds a measure to an attribute (ensures measure exists for use with attribute)
        /// </summary>
        [HttpPost("attributes/{attributeName}/measures")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMeasureByAttribute(
            [FromRoute] string attributeName,
            [FromBody] MeasureRequest request)
        {
            if (string.IsNullOrWhiteSpace(attributeName))
                return BadRequest(new { message = "Attribute name is required" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Measure name is required" });

            var result = await _attributeMeasureServices.AddMeasureByAttributeAsync(attributeName, request.Name);
            if (!result)
                return NotFound(new { message = "Attribute not found or failed to add measure" });

            return Ok(new { 
                message = "Measure added to attribute successfully ✅", 
                attributeName = attributeName,
                measureName = request.Name 
            });
        }
    }

    // =========================================================
    // 🔹 REQUEST DTOs
    // =========================================================
    public class AttributeRequest
    {
        [Required(ErrorMessage = "Attribute name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Attribute name must be between 1 and 100 characters")]
        public string Name { get; set; } = string.Empty;
    }

    public class MeasureRequest
    {
        [Required(ErrorMessage = "Measure name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Measure name must be between 1 and 100 characters")]
        public string Name { get; set; } = string.Empty;
    }
}

