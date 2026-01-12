using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IPresenceService
    {
        Task UserConnectedAsync(string userId, string connectionId);
        Task UserDisconnectedAsync(string userId, string connectionId);
        Task<bool> IsUserOnlineAsync(string userId);
        Task<int> GetConnectionCountAsync(string userId);
        Task<List<string>> GetAllOnlineUsersAsync();
    }
}
