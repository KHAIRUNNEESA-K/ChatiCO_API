using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group> CreateGroupAsync(Group group);
        Task<List<Group>> GetGroupsCreatedByUserAsync(int userId);
        Task<Group?> GetGroupDetailAsync(int groupId);
        Task<Group?> GetGroupByIdAsync(int groupId);
        Task<List<Group>> GetUserGroupsAsync(int userId);
        Task UpdateGroupAsync(Group group);
        Task DeleteGroupAsync(int groupId);
    }
}
