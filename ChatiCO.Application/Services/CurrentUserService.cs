using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Services
{
    public class CurrentUserService : ICurrentUserServices
    {
        public int UserId { get; set; }
        public string UserName { get; set; } 
        public CurrentUserService(IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext?.User;

            if (user != null)
            {
                UserId = int.Parse(
                    user.FindFirst("userId")?.Value ??
                    user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                    "0"
                );

                UserName =
                    user.FindFirst("username")?.Value ??
                    user.FindFirst(ClaimTypes.Name)?.Value ??
                    "Unknown";
            }
        }
    }
}
