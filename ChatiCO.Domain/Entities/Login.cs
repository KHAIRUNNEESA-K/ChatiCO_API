using ChatiCO.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Domain.Entities
{
    public class Login:BaseEntity
    {
        public int LoginId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime? LoginTime { get; set; }

    }
}
