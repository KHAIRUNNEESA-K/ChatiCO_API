using ChatiCO.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IContactService
    {
        Task<object> GetAvailableUsersAsync();
        Task<object> AddContactAsync(int currentUserId, AddContactRequestDto dto);
        Task<object> GetUserContactsAsync(int currentUserId);
        Task<object> BlockContactAsync(int currentUserId, int contactUserId);
        Task<object> UnblockContactAsync(int currentUserId, int contactUserId);
        Task<object> DeleteContactAsync(int contactId, int currentUserId);
        Task<object> GetBlockedContactsAsync(int currentUserId);
        Task<object> GetAllOtherUsersAsync(int currentUserId);
    }
}
