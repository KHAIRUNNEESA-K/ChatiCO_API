using ChatiCO.Application.DTOs;
using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IContactRepository
    {

        Task AddContactAsync(Contacts contact);
        Task<List<User>> GetAvailableUsersAsync();
        Task<List<ContactDto>> GetUserContactsAsync(int userId);
        Task<object> DeleteContactAsync(int contactId, int currentUserId);
        Task<bool> BlockContactAsync(int currentUserId, int contactUserId);
        Task<bool> UnblockContactAsync(int currentUserId, int contactUserId);
        Task<bool> IsBlockedAsync(int userId, int contactUserId);
        Task<ChatUserInfoDto?> GetChatUserInfoAsync(int currentUserId, int contactUserId);
        Task<List<ContactDto>> GetBlockedContactsAsync(int currentUserId);
        Task<List<ContactDto>> GetUsersWhoBlockedMeAsync(int currentUserId);
        Task<List<User>> GetAllOtherUsersAsync(int currentUserId);

    }
}
