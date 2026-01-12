using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class GroupUpdateDto
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public IFormFile? ProfilePic { get; set; }
    }

}
