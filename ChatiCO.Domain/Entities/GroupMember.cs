using ChatiCO.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Domain.Entities
{
    public class GroupMember : BaseEntity
    {
        public int GroupMemberId { get; set; }  
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset JoinedAt { get; set; } = DateTimeOffset.UtcNow;
        public Group Group { get; set; } = default!;
    }
}
