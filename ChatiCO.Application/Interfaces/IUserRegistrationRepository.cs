using ChatiCO.Domain.Entities;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IUserRegistrationRepository
    {
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User?> GetUserByIdAsync(int userId);
    }
}
