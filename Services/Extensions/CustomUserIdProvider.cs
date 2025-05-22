using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AlexSupport.Services.Extensions
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // This should match how you identify users in your auth system
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
