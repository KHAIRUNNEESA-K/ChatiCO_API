using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class GroupCreateDto
    {
        public string GroupName { get; set; } = null!;
        public IFormFile? GroupProfilePic { get; set; }  
        public List<int> MemberIds { get; set; } = new();
    }

}
