// API/Services/GroupNotificationService.cs
using ChatiCO.API.Hubs;
using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

public class GroupNotificationService : IGroupNotificationService
{
    private readonly IHubContext<ChatHub> _hubContext;

    public GroupNotificationService(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyNewGroupAdded(GroupDetailDto group, int[] memberIds, string creatorUsername)
    {
        var payload = new
        {
            GroupId = group.GroupId,
            GroupName = group.GroupName,
            GroupProfilePicUrl = group.GroupProfilePicUrl,
            Members = group.Members,
            ShouldAutoJoin = true
        };

        foreach (var memberId in memberIds.Distinct())
        {
            
            await _hubContext.Clients.User(memberId.ToString())
                .SendAsync("NewGroupAdded", payload);
        }


        foreach (var memberId in memberIds.Distinct())
        {
            var systemPayload = new
            {
                GroupId = group.GroupId,
                Content = $"{creatorUsername} created group '{group.GroupName}' and added you",
                IsSystemMessage = true,
                MessageType = "System",
                SentTime = DateTimeOffset.UtcNow
            };

            await _hubContext.Clients.User(memberId.ToString())
                .SendAsync("ReceiveGroupMessage", systemPayload);
        }
    }


}
