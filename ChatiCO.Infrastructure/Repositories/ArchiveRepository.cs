using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using ChatiCO.Infrastructure.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest;

namespace ChatiCO.Infrastructure.Repositories
{ 
public class ArchiveRepository : IArchiveRepository
{
    private readonly IDbConnection _dbConnection;

    public ArchiveRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ArchiveResponseDto> AddToArchiveAsync(int userId, int contactId)
    {
        var archive = await _dbConnection.QueryFirstOrDefaultAsync<ArchiveResponseDto>(
            "sp_AddToArchive",
            new { UserId = userId, ContactId = contactId },
            commandType: CommandType.StoredProcedure
        );

        return archive;
    }


        public async Task<bool> RemoveFromArchiveAsync(int userId, int contactId, string deletedBy)
        {
            var result = await _dbConnection.QueryFirstAsync<dynamic>(
                "sp_RemoveFromArchive",
                new { UserId = userId, ContactId = contactId, DeletedBy = deletedBy },
                commandType: CommandType.StoredProcedure
            );

            return result.Success == 1;
        }


        public async Task<List<ArchiveResponseDto>> GetArchivedContactsAsync(int userId)
    {
        var archivedContacts = (await _dbConnection.QueryAsync<ArchiveResponseDto>(
            "sp_GetArchivedContacts",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure
        )).ToList();

        return archivedContacts;
    }
}

}
