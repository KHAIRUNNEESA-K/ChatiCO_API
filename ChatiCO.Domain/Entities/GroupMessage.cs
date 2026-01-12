using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Domain.Entities
{
    public class GroupMessage
    {
        public int GroupMessageId { get; set; }  
        public int GroupId { get; set; }
        public int SenderId { get; set; }
        public string? Content { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string MessageType { get; set; } = "Text";
        public bool IsSystemMessage { get; set; } = false;
        public int? TargetUserId { get; set; }

        public DateTimeOffset SentTime { get; set; } = DateTimeOffset.UtcNow;
        public Group Group { get; set; } = default!;
    }
}
