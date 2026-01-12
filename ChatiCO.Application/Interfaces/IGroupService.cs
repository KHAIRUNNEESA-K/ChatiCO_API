using ChatiCO.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IGroupService
    {
        Task<GroupDetailDto> CreateGroupAsync(GroupCreateDto dto, int creatorId);
        Task<List<GroupDetailDto>> GetGroupsCreatedByUserAsync(int userId);
        Task<GroupDetailDto> GetSingleGroupAsync(int groupId);

        Task<GroupDetailDto> UpdateGroupAsync(GroupUpdateDto dto, int userId);
        Task AddMembersAsync(GroupMemberAddDto dto, int userId);
        Task<List<GroupMemberDto>> GetMembersAsync(int groupId);
        Task<GroupMessageDto> SendMessageAsync(GroupMessageSendDto dto);
        Task<List<GroupMessageDto>> GetChatHistoryAsync(int groupId);
        Task DeleteGroupAsync(int groupId, int userId);
        Task<List<GroupDetailDto>> GetGroupsByUserAsync(int userId);
    }
}
