using ChatiCO.Domain.Common;
using System;
using System.Collections.Generic;

namespace ChatiCO.Domain.Entities
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePicturePath { get; set; } = null;

        public string? Bio { get; set; } = null;
        public bool isOnline { get; set; } = false;
        public DateTime? LastSeen { get; set; } = null;
        public bool IsVerified { get; set; } = false;

        public ICollection<Login> Logins { get; set; } = new List<Login>();
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
        public ICollection<Archive> Archives { get; set; } = new List<Archive>();
    }
}
