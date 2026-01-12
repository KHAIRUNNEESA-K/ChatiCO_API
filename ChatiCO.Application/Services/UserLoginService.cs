using ChatiCO.Application.DTOs;
using ChatiCO.Application.Helpers;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using System;
using System.Threading.Tasks;
using Twilio.Exceptions;

namespace ChatiCO.Application.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly IUserRegistrationRepository _repo;
        private readonly IOtpService _otpService;
        private readonly IUserLoginRepository _loginRepo;
        private readonly IAuthService _authService;

        public UserLoginService(
            IUserRegistrationRepository repo,
            IOtpService otpService,
            IUserLoginRepository loginRepo,
            IAuthService authService)
        {
            _repo = repo;
            _otpService = otpService;
            _loginRepo = loginRepo;
            _authService = authService;
        }
        public async Task<object> UserLoginAsync(LoginRequestDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.PhoneNumber))
                return new { success = false, message = "Phone number is required" };

            var user = await _repo.GetUserByPhoneNumberAsync(dto.PhoneNumber);
            if (user == null || !user.IsVerified)
                return new { success = false, message = "User not found or not verified" };

            try
            {
                await _otpService.SendOtpAsync(user.PhoneNumber);
                return new { success = true, message = "OTP sent successfully" };
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Twilio OTP send failed: {ex.Message}");
                return new
                {
                    success = true,
                    message = "OTP could not be sent. Please try again later."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected OTP error: {ex.Message}");
                return new
                {
                    success = true,
                    message = "OTP could not be sent due to an error."
                };
            }
        }
        public async Task<object> VerifyLoginOtpAsync(string otp)
        {
            if (string.IsNullOrEmpty(otp))
                return new { success = false, message = "OTP is required" };

            try
            {
                string? phoneNumber = OtpStorage.GetPhoneNumberByOtp(otp);
                if (phoneNumber == null)
                    return new { success = false, message = "Invalid or expired OTP" };

                var user = await _repo.GetUserByPhoneNumberAsync(phoneNumber);
                if (user == null)
                    return new { success = false, message = "User not found" };

                bool isValid;
                try
                {
                    isValid = await _otpService.VerifyOtpAsync(phoneNumber, otp);
                }
                catch (ApiException ex)
                {
                    Console.WriteLine($"Twilio OTP verification failed: {ex.Message}");
                    return new { success = false, message = "OTP verification failed due to Twilio error." };
                }

                var loginLog = new Login
                {
                    UserId = user.UserId,
                    LoginTime = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = user.Username,
                    IsSuccessful = isValid
                };
                await _loginRepo.AddLoginAsync(loginLog);

                if (!isValid)
                    return new { success = false, message = "Invalid OTP" };

                
                user.ModifiedOn = DateTime.UtcNow;
                user.ModifiedBy = user.Username;
                await _repo.UpdateUserAsync(user);

       
                var token = await _authService.GenerateJwtTokenAsync(user.UserId, user.PhoneNumber, user.Username);

                return new
                {
                    success = true,
                    message = "Login successful",
                    token,
                    user = new
                    {
                        userId = user.UserId,
                        userName = user.Username,
                        phoneNumber = user.PhoneNumber,
                        profilePictureUrl = user.ProfilePicturePath
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login verification error: {ex.Message}");
                return new { success = false, message = "An error occurred during login" };
            }
        }
    }
}
