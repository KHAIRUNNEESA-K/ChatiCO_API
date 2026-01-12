using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRespository _profileRepo;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserServices _currentUser;

        public UserProfileService(
            IUserProfileRespository profileRepo,
            IFileStorageService fileStorageService,
            ICurrentUserServices currentUser)
        {
            _profileRepo = profileRepo;
            _fileStorageService = fileStorageService;
            _currentUser = currentUser;
        }

        public async Task<UserProfileDto?> GetProfileAsync()
        {
            int userId = _currentUser.UserId;

            var user = await _profileRepo.GetUserByIdAsync(userId);

            if (user == null) return null;

            return new UserProfileDto
            {
                UserId = user.UserId,
                UserName = user.Username,
                PhoneNumber = user.PhoneNumber,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePicturePath
            };
        }

        public async Task<bool> UpdateNameAsync(string newName)
        {
            int userId = _currentUser.UserId;
            var user = await _profileRepo.GetUserByIdAsync(userId);

            if (user == null) return false;

            user.Username = newName;

            await _profileRepo.UpdateUserProfileAsync(user);
            return true;
        }

        public async Task<bool> UpdateBioAsync(string bio)
        {
            int userId = _currentUser.UserId;
            var user = await _profileRepo.GetUserByIdAsync(userId);

            if (user == null) return false;

            user.Bio = bio;

            await _profileRepo.UpdateUserProfileAsync(user);
            return true;
        }

        public async Task<bool> UpdateProfilePictureAsync(IFormFile file)
        {
            int userId = _currentUser.UserId;
            var user = await _profileRepo.GetUserByIdAsync(userId);

            if (user == null) return false;
            string filePath = await _fileStorageService.SaveProfilePictureAsync(file, userId);

            user.ProfilePicturePath = filePath;

            await _profileRepo.UpdateUserProfileAsync(user);
            return true;
        }
    }
}
