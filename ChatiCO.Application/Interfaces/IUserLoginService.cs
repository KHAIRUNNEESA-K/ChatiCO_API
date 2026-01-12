using ChatiCO.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IUserLoginService
    {
        Task<object> UserLoginAsync(LoginRequestDto dto);
        Task<object> VerifyLoginOtpAsync(string otp);
    }
}
