using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Domain.Common
{
    public abstract class BaseEntity
    {
        public string? CreatedBy { get; set; } = null;
        public DateTime? CreatedOn { get; set; } = null;
        public string? ModifiedBy { get; set; } = null;
        public DateTime? ModifiedOn { get; set; } 
        public string? DeletedBy { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
    }
}
