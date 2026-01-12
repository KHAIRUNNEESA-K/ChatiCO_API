using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class ChatUserInfoDto
    {
        public int ContactUserId { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
