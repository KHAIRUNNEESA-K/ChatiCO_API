using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class GroupDetailDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string? GroupProfilePicUrl { get; set; }
        public List<GroupMemberDto> Members { get; set; } = new();
        public List<GroupMessageDto> Messages { get; set; } = new();
    }

}
