using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class GroupMessageSendDto
    {
        public int GroupId { get; set; }
        public int SenderId { get; set; }
        public string MessageType { get; set; } = "Text"; 
        public string? TextContent { get; set; } = null;
        public IFormFile? File { get; set; } = null;
        public string? TempId { get; set; } = null; 
    }

}
