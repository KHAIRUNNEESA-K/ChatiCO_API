using ChatiCO.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileDto?> GetProfileAsync();
        Task<bool> UpdateNameAsync(string newName);
        Task<bool> UpdateBioAsync(string bio);
        Task<bool> UpdateProfilePictureAsync(IFormFile file);
    }
}
