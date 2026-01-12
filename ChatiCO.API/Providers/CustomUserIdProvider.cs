using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        
        var userIdClaim = connection.User?.FindFirst("userId")?.Value;

        if (!string.IsNullOrEmpty(userIdClaim))
            return userIdClaim;

        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
