using ChatiCO.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChatiCO.Domain.Entities;
using Dapper;
using ChatiCO.Application.Interfaces;

namespace ChatiCO.Infrastructure.Repositories
{
    public class UserRegistrationRepository : IUserRegistrationRepository
    {
        private readonly ChatiCODbContext _context;
        private readonly IDbConnection _dbConnection;

        public UserRegistrationRepository(ChatiCODbContext context)
        {
            _context = context;
            _dbConnection = _context.Database.GetDbConnection();
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PhoneNumber", phoneNumber);

            var result=await _dbConnection.QueryFirstOrDefaultAsync<User>("sp_GetUserByPhone", parameters,commandType:CommandType.StoredProcedure);
            return result;
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            var result = await _dbConnection.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserById",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }


    }
}
