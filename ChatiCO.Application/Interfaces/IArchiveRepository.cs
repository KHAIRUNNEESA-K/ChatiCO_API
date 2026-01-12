using ChatiCO.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IArchiveRepository
    {
        Task<ArchiveResponseDto> AddToArchiveAsync(int userId, int contactId);
        Task<bool> RemoveFromArchiveAsync(int userId, int contactId, string deletedBy);
        Task<List<ArchiveResponseDto>> GetArchivedContactsAsync(int userId);
    }

}
