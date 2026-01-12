using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IUserProfileRespository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task UpdateUserProfileAsync(User user);
    }
}
