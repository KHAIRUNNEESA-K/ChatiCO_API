using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class GroupSummaryDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string? GroupProfilePicUrl { get; set; }
        public string? LastMessage { get; set; }
        public string? LastMessageSenderName { get; set; }
        public DateTimeOffset? LastMessageTime { get; set; }
        public int UnreadMessagesCount { get; set; }
    }

}
