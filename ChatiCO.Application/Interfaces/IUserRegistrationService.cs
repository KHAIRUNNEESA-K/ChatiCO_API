using ChatiCO.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IUserRegistrationService
    {
        Task<object> RegisterUserAsync(RegisterRequestDto dto);
        Task<object> VerifyRegistrationAsync(string otp);
        Task<object> GetUserByIdAsync(int userId);

    }
}
