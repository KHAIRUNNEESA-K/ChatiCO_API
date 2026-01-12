using System;
using System.Threading.Tasks;
using ChatiCO.Application.Helpers;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;

namespace ChatiCO.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly ITwilioService _twilioService;

        public OtpService(ITwilioService twilioService)
        {
            _twilioService = twilioService;
        }

        public async Task<object> SendOtpAsync(string phoneNumber)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            OtpStorage.SaveOtp(phoneNumber, otp);
            Console.WriteLine(otp);
            await _twilioService.SendOtpAsync(phoneNumber, otp);

            return new
            {
                success = true,
                message = $"OTP sent to {phoneNumber}"
            };
        }

        public async Task<bool> VerifyOtpAsync(string phoneNumber, string otp)
        {
            return  OtpStorage.VerifyOtp(phoneNumber, otp);
        }
    }

}

