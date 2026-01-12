using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatiCO.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserLoginService _userLoginService;
        public UserLoginController(IUserLoginService userLoginService)
        {
            _userLoginService = userLoginService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (loginDto == null)
                return BadRequest(new { success = false, message = "Invalid request Data" });

            var result = await _userLoginService.UserLoginAsync(loginDto);
            return new OkObjectResult(result);
        }
        [HttpPost("verify-login-otp")]
        public async Task<IActionResult> VerifyLoginOtp([FromBody] VerifyLoginOtpdto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.otp))
                return BadRequest(new { success = false, message = "OTP is required" });

            var result = await _userLoginService.VerifyLoginOtpAsync(dto.otp);
            return Ok(result);
        }

    }
}
