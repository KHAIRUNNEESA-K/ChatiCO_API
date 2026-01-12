using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using ChatiCO.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Repositories
{
    public class UserLoginRepository :IUserLoginRepository
    {
        private readonly ChatiCODbContext _context;
        private readonly IDbConnection _dbConnection;
        public UserLoginRepository(ChatiCODbContext context)
        {
            _context = context;
            _dbConnection = _context.Database.GetDbConnection();
        }
        public async Task AddLoginAsync(Domain.Entities.Login login)
        {
            await _context.Logins.AddAsync(login);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateLoginAsync(Domain.Entities.Login login)
        {
            _context.Logins.Update(login);
            await _context.SaveChangesAsync();
        }
        public async Task<Domain.Entities.Login?> GetLoginByIdAsync(int loginId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LoginId", loginId);
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Login>("sp_GetLoginById", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<Domain.Entities.Login>> GetLoginsByUserIdAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            var result = await _dbConnection.QueryAsync<Login>("sp_GetLoginsByUserId", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }

}
