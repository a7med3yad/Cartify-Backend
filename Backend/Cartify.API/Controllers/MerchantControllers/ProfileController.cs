using Cartify.Application.Contracts.ProfileDtos;
using Cartify.Application.Services.Interfaces.Merchant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cartify.API.Controllers.MerchantControllers
{
    [Route("api/merchant/profile")]
    [ApiController]
    [Authorize(Roles = "Merchant")]
    public class ProfileController : ControllerBase
    {
        private readonly IMerchantProfileServices _profileServices;
        public ProfileController(IMerchantProfileServices _profileServices)
        {
            this._profileServices = _profileServices;
        }

        // =========================================================
        // 🔹 GET PROFILE BY USER ID
        // =========================================================
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileByUserId([FromRoute] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { message = "User ID is required" });

            var profile = await _profileServices.GetProfileByUserIdAsync(userId);
            if (profile == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(profile);
        }

        // =========================================================
        // 🔹 GET CURRENT USER PROFILE
        // =========================================================
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { message = "User ID not found in token" });

            var profile = await _profileServices.GetProfileByUserIdAsync(userId);
            if (profile == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(profile);
        }

        // =========================================================
        // 🔹 UPDATE PROFILE
        // =========================================================
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateMerchantProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            var result = await _profileServices.UpdateProfileAsync(dto);
            if (!result)
                return BadRequest(new { message = "Failed to update profile" });

            return Ok(new { message = "Profile updated successfully", success = true });
        }

        // =========================================================
        // 🔹 UPDATE STORE INFO
        // =========================================================
        [HttpPut("store")]
        public async Task<IActionResult> UpdateStoreInfo([FromBody] UpdateStoreInfoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            var result = await _profileServices.UpdateStoreInfoAsync(dto);
            if (!result)
                return BadRequest(new { message = "Failed to update store info" });

            return Ok(new { message = "Store info updated successfully", success = true });
        }

        // =========================================================
        // 🔹 CHANGE PASSWORD
        // =========================================================
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data", errors = ModelState });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { message = "User ID not found in token" });

            var result = await _profileServices.ChangePasswordAsync(userId, dto.OldPassword, dto.NewPassword);
            if (!result)
                return BadRequest(new { message = "Failed to change password. Please check your old password." });

            return Ok(new { message = "Password changed successfully", success = true });
        }
    }

    public class ChangePasswordDto
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
