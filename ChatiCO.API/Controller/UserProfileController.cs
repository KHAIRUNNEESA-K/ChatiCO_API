using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatiCO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(IUserProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _profileService.GetProfileAsync();
            return Ok(profile);
        }

        [HttpPatch("update-name")]
        public async Task<IActionResult> UpdateName([FromBody] UpdateNameDto dto)
        {
            var result = await _profileService.UpdateNameAsync(dto.NewName);
            return Ok(new { success = result });
        }

        [HttpPatch("update-bio")]
        public async Task<IActionResult> UpdateBio([FromBody] UpdateBioDto dto)
        {
            var result = await _profileService.UpdateBioAsync(dto.Bio);
            return Ok(new { success = result });
        }

        [HttpPatch("update-profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var result = await _profileService.UpdateProfilePictureAsync(file);
            return Ok(new { success = result });
        }
    }
}
