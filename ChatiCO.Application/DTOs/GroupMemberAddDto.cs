using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class GroupMemberAddDto
    {
        public int GroupId { get; set; }
        public List<int> MemberIds { get; set; } = new();
    }

}
