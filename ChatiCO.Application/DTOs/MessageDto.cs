using System;
using System.Text.Json.Serialization;

namespace ChatiCO.Application.DTOs
{
    public class MessageDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageType { get; set; } = "Text";
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? TempId { get; set; }

        public DateTimeOffset? SentTime { get; set; } = DateTimeOffset.UtcNow;
    };
}


