using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtTokenAsync(int userId, string phoneNumber,string username);
    }
}
