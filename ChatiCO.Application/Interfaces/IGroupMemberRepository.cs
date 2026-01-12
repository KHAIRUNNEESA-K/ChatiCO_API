using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IGroupMemberRepository
    {
        Task AddMembersAsync(List<GroupMember> members);
        Task<List<GroupMember>> GetMembersByGroupIdAsync(int groupId);
        Task<bool> IsUserInGroupAsync(int groupId, int userId);
        Task RemoveMemberAsync(int groupId, int userId);
        Task<List<Group>> GetGroupsByUserIdAsync(int userId);
    }
}
