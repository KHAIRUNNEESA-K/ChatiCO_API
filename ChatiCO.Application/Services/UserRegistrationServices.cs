using AutoMapper;
using ChatiCO.Application.DTOs;
using ChatiCO.Application.Helpers;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using System;
using System.Threading.Tasks;
using Twilio.Exceptions;

namespace ChatiCO.Application.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserRegistrationRepository _repo;
        private readonly IOtpService _otpService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserRegistrationService(
            IUserRegistrationRepository repo,
            IOtpService otpService,
            IMapper mapper,
            IAuthService authService)
        {
            _repo = repo;
            _otpService = otpService;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<object> RegisterUserAsync(RegisterRequestDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.PhoneNumber))
                return new { success = false, message = "Phone number is required" };

            var existingUser = await _repo.GetUserByPhoneNumberAsync(dto.PhoneNumber);
            if (existingUser != null && existingUser.IsVerified)
                return new { success = false, message = "User already exists" };

            User user;
            if (existingUser == null)
            {
                user = _mapper.Map<User>(dto);
                user.IsVerified = false;
                user.CreatedOn = DateTime.UtcNow;
                user.CreatedBy=user.Username;

                await _repo.AddUserAsync(user);
            }
            else
            {
                user = existingUser;
            }
            try
            {
                await _otpService.SendOtpAsync(user.PhoneNumber);
                return new { success = true, message = "OTP sent" };
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Twilio OTP failed: {ex.Message}");
                return new
                {
                    success = false,
                    message = "User registered but OTP could not be sent. Verify your phone later."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected OTP error: {ex.Message}");
                return new
                {
                    success = false,
                    message = "User registered but OTP could not be sent due to an error."
                };
            }
        }
        public async Task<object> VerifyRegistrationAsync(string otp)
        {
            if (string.IsNullOrEmpty(otp))
                return new { success = false, message = "OTP is required" };

            try
            {
                string? phoneNumber = OtpStorage.GetPhoneNumberByOtp(otp);
                if (phoneNumber == null)
                    return new { success = false, message = "OTP expired or invalid" };

                var user = await _repo.GetUserByPhoneNumberAsync(phoneNumber);
                if (user == null)
                    return new { success = false, message = "User not found" };

                bool isValid = await _otpService.VerifyOtpAsync(phoneNumber, otp);
                if (!isValid)
                    return new { success = false, message = "Invalid OTP" };

                user.IsVerified = true;
                
                await _repo.UpdateUserAsync(user);

                var token = await _authService.GenerateJwtTokenAsync(user.UserId, user.PhoneNumber, user.Username);

                return new
                {
                    success = true,
                    message = "Registration verified",
                    token,
                    user = new
                    {
                        userId = user.UserId,
                        userName = user.Username,
                        phoneNumber = user.PhoneNumber
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Verification error: {ex.Message}");
                return new { success = false, message = "An error occurred during verification" };
            }
        }
            public async Task<object> GetUserByIdAsync(int userId)
            {
            if (userId <= 0)
                return new { success = false, message = "Invalid UserId" };

            var user = await _repo.GetUserByIdAsync(userId);

            if (user == null)
                return new { success = false, message = "User not found" };

            return new
            {
                success = true,
                user = new
                {
                    userId = user.UserId,
                    userName = user.Username,
                    phoneNumber = user.PhoneNumber,
                    bio = user.Bio,
                    isOnline = user.isOnline,
                    lastSeen = user.LastSeen,
                    isVerified = user.IsVerified
                }
            };
        }

    }
}

