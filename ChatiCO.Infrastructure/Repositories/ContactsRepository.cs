using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using ChatiCO.Infrastructure.Data;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Repositories
{
    public class ContactsRepository : IContactRepository
    {
        private readonly ChatiCODbContext _context;
        private readonly IDbConnection _dbConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactsRepository(
            ChatiCODbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _dbConnection = _context.Database.GetDbConnection();
        }
        public async Task AddContactAsync(Contacts contact)
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAvailableUsersAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.
                User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in token.");

            int currentUserId = int.Parse(userIdClaim);

            var parameters = new DynamicParameters();
            parameters.Add("@CurrentUserId", currentUserId);

            var users = await _dbConnection.QueryAsync<User>(
                "sp_GetAvailableUsers",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return users.ToList();
        }
        public async Task<List<ContactDto>> GetUserContactsAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CurrentUserId", userId);

            var contacts = await _dbConnection.QueryAsync<ContactDto>(
                "sp_GetUserContacts",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return contacts.ToList();
        }
        public async Task<object> DeleteContactAsync(int contactId, int currentUserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", currentUserId);
            parameters.Add("@ContactUserId", contactId);

            var result = await _dbConnection.QueryFirstAsync<(bool Success, string Message)>(
                "sp_DeleteContact",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return new { success = result.Success, message = result.Message };

        }
        public async Task<ChatUserInfoDto?> GetChatUserInfoAsync(int currentUserId, int contactUserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CurrentUserId", currentUserId);
            parameters.Add("@ContactUserId", contactUserId);

            return await _dbConnection.QueryFirstOrDefaultAsync<ChatUserInfoDto>(
                "sp_GetChatUserInfo",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> BlockContactAsync(int currentUserId, int contactUserId)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.UserId == currentUserId && c.ContactUserId == contactUserId);

            if (contact == null) return false;

            if (contact.IsBlocked) return true;

            contact.IsBlocked = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnblockContactAsync(int currentUserId, int contactUserId)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.UserId == currentUserId && c.ContactUserId == contactUserId);

            if (contact == null) return false;

            if (!contact.IsBlocked) return true;

            contact.IsBlocked = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsBlockedAsync(int userId, int contactUserId)
        {
            return await _context.Contacts
                .AnyAsync(c =>
                    c.UserId == userId &&
                    c.ContactUserId == contactUserId &&
                    c.IsBlocked == true
                );
        }

        public async Task<List<ContactDto>> GetBlockedContactsAsync(int currentUserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CurrentUserId", currentUserId);

            var blockedContacts = await _dbConnection.QueryAsync<ContactDto>(
                "sp_GetBlockedContacts",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return blockedContacts.ToList();
        }
        public async Task<List<ContactDto>> GetUsersWhoBlockedMeAsync(int currentUserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CurrentUserId", currentUserId);

            var result = await _dbConnection.QueryAsync<ContactDto>(
                "sp_GetUsersWhoBlockedMe",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }


        public async Task<List<User>> GetAllOtherUsersAsync(int currentUserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CurrentUserId", currentUserId);

            var users = await _dbConnection.QueryAsync<User>(
                "sp_GetAllOtherUsers",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return users.ToList();
        }

    }

}
