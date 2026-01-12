using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class ContactDto 
    {

        public int ContactUserId { get; set; }       
        public string ContactUsername { get; set; } = string.Empty;
        public string? ProfilePicturePath { get; set; }
        public bool IsBlocked { get; set; }         

        public int BlockingUserId { get; set; }
        public bool IsOnline { get; set; }
    }
}
