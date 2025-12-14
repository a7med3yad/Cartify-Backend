using Cartify.Application.Contracts.ProfileDtos;
using Cartify.Application.Services.Interfaces;
using Cartify.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cartify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            try
            {
                var user = await _profileService.GetUserProfileAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                // Return only the data we need
                var userProfile = new
                {
                    id = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                    birthDate = user.BirthDate,
                  
                };

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] Cartify.Application.Contracts.ProfileDtos.UpdateProfileDto request)
        {
            try
            {
                // Get existing user
                var existingUser = await _profileService.GetUserProfileAsync(userId);
                if (existingUser == null)
                    return NotFound(new { message = "User not found" });

                // Update only the allowed fields
                existingUser.FirstName = request.FirstName;
                existingUser.LastName = request.LastName;
                existingUser.PhoneNumber = request.PhoneNumber;
                existingUser.BirthDate = request.BirthDate;

                var result = await _profileService.UpdateUserProfileAsync(existingUser);
                if (!result)
                    return BadRequest(new { message = "Update failed" });

                return Ok(new { message = "Profile updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}
