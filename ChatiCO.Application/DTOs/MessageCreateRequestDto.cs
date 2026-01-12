using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class MessageCreateRequestDto
    {
        public int ReceiverId { get; set; }
        public string MessageType { get; set; } = "Text";
        public string? Content { get; set; }
        public IFormFile? File { get; set; }
        public string? TempId { get; set; }
    }
}
