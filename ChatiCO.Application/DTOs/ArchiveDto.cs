using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.DTOs
{
    public class ArchiveDto
    {
        public int ArchiveId { get; set; }
        public int ContactId { get; set; }
        public string ContactUsername { get; set; } = null!;
        public string? ContactProfilePicture { get; set; }
    }
}

