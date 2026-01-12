using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveProfilePictureAsync(IFormFile file, int userId);
        Task DeleteFileAsync(string relativePath);
    }
}
