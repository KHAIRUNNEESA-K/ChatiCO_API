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
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly ChatiCODbContext _context;
        private readonly IDbConnection _db;

        public GroupMemberRepository(ChatiCODbContext context)
        {
            _context = context;
            _db = _context.Database.GetDbConnection();
        }

        public async Task AddMembersAsync(List<GroupMember> members)
        {
            await _context.GroupMembers.AddRangeAsync(members);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GroupMember>> GetMembersByGroupIdAsync(int groupId)
        {
            var param = new DynamicParameters();
            param.Add("@GroupId", groupId);

            var result = await _db.QueryAsync<GroupMember>(
                "sp_GetGroupMembers",
                param,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<bool> IsUserInGroupAsync(int groupId, int userId)
        {
            return await _context.GroupMembers
                .AnyAsync(x => x.GroupId == groupId && x.UserId == userId);
        }

        public async Task RemoveMemberAsync(int groupId, int userId)
        {
            var member = await _context.GroupMembers
                .FirstOrDefaultAsync(x => x.GroupId == groupId && x.UserId == userId);

            if (member != null)
            {
                _context.GroupMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Group>> GetGroupsByUserIdAsync(int userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);

            var groups = await _db.QueryAsync<Group>(
                "sp_GetUserGroups",  
                param,
                commandType: CommandType.StoredProcedure
            );

            return groups.ToList();
        }

    }
}
