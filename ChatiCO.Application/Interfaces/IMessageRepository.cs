using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ChatiCO.Application.Interfaces
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task UpdateMessageAsync(Message message);
        Task DeleteMessageAsync(int messageId);
        Task<List<Message>> GetMessagesBetweenUsersAsync(int senderId, int receiverId);
        Task<Message?> GetLastMessageAsync(int senderId, int receiverId);
        Task<int> GetUnreadMessagesCountAsync(int userId, int contactId);
        Task MarkMessageAsReadAsync(int messageId);
        Task MarkMessageAsDeliveredAsync(int messageId);

    }
}
