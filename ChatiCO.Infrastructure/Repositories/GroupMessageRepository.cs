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
    public class GroupMessageRepository : IGroupMessageRepository
    {
        private readonly ChatiCODbContext _context;
        private readonly IDbConnection _db;

        public GroupMessageRepository(ChatiCODbContext context)
        {
            _context = context;
            _db = _context.Database.GetDbConnection();
        }

        public async Task AddMessageAsync(GroupMessage message)
        {
            await _context.GroupMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GroupMessage>> GetGroupMessagesAsync(int groupId)
        {
            var param = new DynamicParameters();
            param.Add("@GroupId", groupId);

            var result = await _db.QueryAsync<GroupMessage>(
                "sp_GetGroupMessages",
                param,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<GroupMessage?> GetLastGroupMessageAsync(int groupId)
        {
            var param = new DynamicParameters();
            param.Add("@GroupId", groupId);

            var result = await _db.QueryFirstOrDefaultAsync<GroupMessage>(
                "sp_GetLastGroupMessage",
                param,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
