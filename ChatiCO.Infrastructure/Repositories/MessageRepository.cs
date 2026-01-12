using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using ChatiCO.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Repositories
{
    public class MessagesRepository : IMessageRepository
    {
        private readonly ChatiCODbContext _context;
        private readonly IDbConnection _dbConnection;

        public MessagesRepository(ChatiCODbContext context)
        {
            _context = context;
            _dbConnection = _context.Database.GetDbConnection();
        }
        public async Task AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            Console.WriteLine("C# SentTime Saved: " + message.SentTime?.ToString("O"));
        }

        public async Task UpdateMessageAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMessageAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Message>> GetMessagesBetweenUsersAsync(int senderId, int receiverId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SenderId", senderId);
            parameters.Add("@ReceiverId", receiverId);

            var messages = await _dbConnection.QueryAsync<Message>(
                "sp_GetMessagesBetweenUsers",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return messages.ToList();
        }
        public async Task<Message?> GetLastMessageAsync(int senderId, int receiverId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SenderId", senderId);
            parameters.Add("@ReceiverId", receiverId);

            var message = await _dbConnection.QueryFirstOrDefaultAsync<Message>(
                "sp_GetLastMessage",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            Console.WriteLine("SQL SentTime Read: " + message?.SentTime?.ToString("O"));

            return message;

        }

        public async Task<int> GetUnreadMessagesCountAsync(int userId, int contactId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@ContactId", contactId);

            var count = await _dbConnection.ExecuteScalarAsync<int>(
                "sp_GetUnreadMessagesCount",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return count;
        }
        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@MessageId", messageId);

            await _dbConnection.ExecuteAsync(
                "sp_MarkMessageAsRead",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task MarkMessageAsDeliveredAsync(int messageId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@MessageId", messageId);

            await _dbConnection.ExecuteAsync(
                "sp_MarkMessageAsDelivered",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
