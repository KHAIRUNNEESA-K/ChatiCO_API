using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface ITwilioService
    {
        Task SendOtpAsync(string phoneNumber, string otp);
    }
}
