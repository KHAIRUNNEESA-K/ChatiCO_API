using ChatiCO.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Interfaces
{
    public interface IGroupNotificationService
    {
        Task NotifyNewGroupAdded(GroupDetailDto group, int[] memberIds, string creatorUsername);
    }
}
