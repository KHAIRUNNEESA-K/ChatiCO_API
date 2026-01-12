using ChatiCO.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IArchiveService
    {

        Task<object> AddArchiveAsync(int currentUserId, ArchiveRequestDto dto, string currentUsername);
        Task<object> RemoveArchiveAsync(int currentUserId, int contactId);
        Task<object> GetArchivedChatsAsync(int currentUserId);
    }
}
