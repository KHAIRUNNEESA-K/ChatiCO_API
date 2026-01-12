using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IGroupMessageRepository
    {
        Task AddMessageAsync(GroupMessage message);
        Task<List<GroupMessage>> GetGroupMessagesAsync(int groupId);
        Task<GroupMessage?> GetLastGroupMessageAsync(int groupId);
    }
}
