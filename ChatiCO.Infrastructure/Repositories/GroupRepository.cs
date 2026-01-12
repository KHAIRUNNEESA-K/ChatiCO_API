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
    public class GroupRepository : IGroupRepository
    {
        private readonly ChatiCODbContext _context;
        private readonly IDbConnection _db;

        public GroupRepository(ChatiCODbContext context)
        {
            _context = context;
            _db = _context.Database.GetDbConnection();
        }

        public async Task<Group> CreateGroupAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            return group;
        }
        public async Task<List<Group>> GetGroupsCreatedByUserAsync(int userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);

            var groups = await _db.QueryAsync<Group>(
                "sp_GetGroupsCreatedByUser",
                param,
                commandType: CommandType.StoredProcedure
            );

            return groups.ToList();
        }


        public async Task<Group?> GetGroupByIdAsync(int groupId)
        {
            var param = new DynamicParameters();
            param.Add("@GroupId", groupId);

            var group = await _db.QueryFirstOrDefaultAsync<Group>(
                "sp_GetGroupById",
                param,
                commandType: CommandType.StoredProcedure
            );

            return group;
        }
        public async Task<Group?> GetGroupDetailAsync(int groupId)
        {
            var param = new DynamicParameters();
            param.Add("@GroupId", groupId);

            var group = await _db.QueryFirstOrDefaultAsync<Group>(
                "sp_GetGroupDetail",
                param,
                commandType: CommandType.StoredProcedure
            );

            return group;
        }

        public async Task<List<Group>> GetUserGroupsAsync(int userId)
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

        public async Task UpdateGroupAsync(Group group)
        {
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGroupAsync(int groupId)
        {
            var entity = await _context.Groups.FindAsync(groupId);
            if (entity != null)
            {
                _context.Groups.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
