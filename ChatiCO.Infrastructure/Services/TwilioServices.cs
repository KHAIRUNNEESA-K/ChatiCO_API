using ChatiCO.Application.Interfaces;
using ChatiCO.Application.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ChatiCO.Infrastructure.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly TwilioSettings _settings;

        public TwilioService(IOptions<TwilioSettings> settings)
        {
            _settings = settings.Value;
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        }

        public async Task SendOtpAsync(string phoneNumber, string otp)
        {
            if (!phoneNumber.StartsWith("+"))
            {
                phoneNumber = $"+91{phoneNumber}";
            }
            var message = await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_settings.FromNumber),
                body: $"Your ChatiCO OTP is {otp}. It expires in 2 minutes.");

            Console.WriteLine($"OTP sent to {phoneNumber}. Message SID: {message.Sid}");
        }
    }
}
