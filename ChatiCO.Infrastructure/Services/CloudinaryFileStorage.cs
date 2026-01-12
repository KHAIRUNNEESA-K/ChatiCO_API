using ChatiCO.Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Services
{
    public class CloudinaryFileStorage : ICloudinaryFileStorage
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryFileStorage(IConfiguration config)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "chat_images"
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result == null || result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                
                throw new Exception($"Cloudinary upload failed: {result?.Error?.Message ?? "unknown"}");
            }

            return result.SecureUrl?.ToString() ?? throw new Exception("Cloudinary returned empty URL.");
        }

    }
}

