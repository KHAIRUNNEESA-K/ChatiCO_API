using ChatiCO.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Domain.Entities
{
    public class Group : BaseEntity
    {
        public int GroupId { get; set; } 
        public string Name { get; set; } = default!;
        public string? ProfileImageUrl { get; set; }
        public int CreatedByUserId { get; set; }
        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public ICollection<GroupMessage> Messages { get; set; } = new List<GroupMessage>();
    }
}
