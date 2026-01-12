using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatiCO.Application.Helpers
{
    public static class OtpStorage
    {
        private static readonly Dictionary<string, (string otp, DateTime expiresAt)> _store = new();
        public static void SaveOtp(string phone, string otp)
        {
            _store[phone] = (otp, DateTime.UtcNow.AddMinutes(2));
        }

        public static bool VerifyOtp(string phone, string otp)
        {
            if (!_store.TryGetValue(phone, out var data))
                return false;

            if (DateTime.UtcNow > data.expiresAt)
            {
                _store.Remove(phone);
                return false;
            }

            bool match = data.otp == otp;

            if (match)
                _store.Remove(phone);

            return match;
        }


        public static string? GetPhoneNumberByOtp(string otp)
        {
            var entry = _store.FirstOrDefault(x => x.Value.otp == otp);

            if (entry.Key == null)
                return null;

     
            if (DateTime.UtcNow > entry.Value.expiresAt)
            {
                _store.Remove(entry.Key);
                return null;
            }

            return entry.Key;
        }
    }
}
