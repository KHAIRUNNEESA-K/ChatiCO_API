using ChatiCO.Application.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatiCO.API.Services
{
    public class InMemoryPresenceService : IPresenceService
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _connections = new();

        public Task UserConnectedAsync(string userId, string connectionId)
        {
            var set = _connections.GetOrAdd(userId, _ => new HashSet<string>());
            lock (set)
            {
                set.Add(connectionId);
            }
            return Task.CompletedTask;
        }

        public Task UserDisconnectedAsync(string userId, string connectionId)
        {
            if (_connections.TryGetValue(userId, out var set))
            {
                lock (set)
                {
                    set.Remove(connectionId);
                    if (set.Count == 0)
                        _connections.TryRemove(userId, out _);
                }
            }
            return Task.CompletedTask;
        }

        public Task<bool> IsUserOnlineAsync(string userId)
        {
            return Task.FromResult(_connections.ContainsKey(userId));
        }

        public Task<int> GetConnectionCountAsync(string userId)
        {
            if (_connections.TryGetValue(userId, out var set))
                return Task.FromResult(set.Count);

            return Task.FromResult(0);
        }
        public Task<List<string>> GetAllOnlineUsersAsync()
        {
            // return only users that have at least 1 connection
            var onlineUsers = _connections
                .Where(kvp => kvp.Value.Count > 0)
                .Select(kvp => kvp.Key)
                .ToList();

            return Task.FromResult(onlineUsers);
        }
    }
}
