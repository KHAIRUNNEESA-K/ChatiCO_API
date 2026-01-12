using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface ICurrentUserServices
    {
        int UserId { get; }
        string UserName { get; }
    }
    
}

