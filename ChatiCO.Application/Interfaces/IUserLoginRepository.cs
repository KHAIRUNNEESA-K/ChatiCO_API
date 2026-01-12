using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IUserLoginRepository
    {
        Task AddLoginAsync(Login login);
        Task UpdateLoginAsync(Login login);
        Task<Login?> GetLoginByIdAsync(int loginId);
        Task<IEnumerable<Login>> GetLoginsByUserIdAsync(int userId);
    }
}
