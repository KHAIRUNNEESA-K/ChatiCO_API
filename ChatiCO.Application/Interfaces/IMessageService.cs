using ChatiCO.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IMessageService
    {
        Task<object> SendMessageAsync(int senderId, MessageCreateRequestDto request);
        Task<object> GetMessagesBetweenUsersAsync(int senderId, int receiverId);
        Task<object> GetLastMessageAsync(int senderId, int receiverId);
        Task<object> MarkMessageAsReadAsync(int messageId);
        Task<object> GetUnreadMessagesCountAsync(int userId, int contactId);
        Task<object> MarkMessageAsDeliveredAsync(int messageId);
    }
}
