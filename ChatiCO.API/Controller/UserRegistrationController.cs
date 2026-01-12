using ChatiCO.Application.DTOs;
using ChatiCO.Application.Helpers;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatiCO.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class UserRegistrationController : ControllerBase
    {
        private readonly IUserRegistrationService _userService;
        public UserRegistrationController(IUserRegistrationService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto userDto)
        {
            if (userDto == null)
                return BadRequest(new { success = false, message = "Invalid request data" });

            var result = await _userService.RegisterUserAsync(userDto);
            return Ok(result);
        }
        
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyRegistrationAsync([FromBody] VerifyRegistrationOtpDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Otp))
                return BadRequest(new { success = false, message = "OTP is required" });

            var result = await _userService.VerifyRegistrationAsync(dto.Otp);

            return Ok(result);
        }
        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return Ok(result);
        }



    }
}
