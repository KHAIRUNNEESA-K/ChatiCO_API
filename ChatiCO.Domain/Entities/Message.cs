using ChatiCO.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Domain.Entities
{
    public class Message : BaseEntity
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageType { get; set; } = "Text"; 
        public byte[]? Content { get; set; } = null;
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }            
        public string? FileType { get; set; }
        public string? TempId { get; set; }
        public DateTimeOffset? SentTime { get; set; } = DateTimeOffset.UtcNow;

        public DateTime? DeliveredTime { get; set; }     
        public DateTime? ReadTime { get; set; }
    }
}
