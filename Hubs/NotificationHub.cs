using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Collections.Concurrent;
using AlexSupport.Services.Extensions;
using AlexSupport.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;

namespace AlexSupport.Hubs
{
    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, UserInfo> ConnectedUsers = new();
        private readonly ILogger<NotificationHub> _logger;
        private readonly ITicketChatService ticketChatService;
        private readonly IAppUserRepoistory _IUser;
        private readonly ITicketRepository _ITicket;
        public NotificationHub(ILogger<NotificationHub> logger, ITicketChatService ticketChatService, IAppUserRepoistory _IUser, ITicketRepository iTicket)
        {
            _logger = logger;
            this.ticketChatService = ticketChatService;
            this._IUser = _IUser;
            _ITicket = iTicket;
        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = Context.UserIdentifier;
            var claims = Context.User?.Claims.ToDictionary(c => c.Type, c => c.Value);

            var userInfo = new UserInfo
            {
                ConnectionId = connectionId,
                UserId = userId,
                Claims = claims
            };

            ConnectedUsers[connectionId] = userInfo;

            _logger.LogInformation($"User connected: {userId ?? "Anonymous"} | ConnectionId: {connectionId}");

            if (Context.User?.Identity?.IsAuthenticated == true)
            {
                foreach (var claim in claims ?? new Dictionary<string, string>())
                {
                    _logger.LogDebug($"Claim: {claim.Key} = {claim.Value}");
                }
            }
            else
            {
                _logger.LogWarning("Unauthenticated connection");
            }

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedUsers.TryRemove(Context.ConnectionId, out _);
            _logger.LogInformation($"User disconnected: {Context.ConnectionId}");

            if (exception != null)
                _logger.LogError(exception, "Disconnection due to error");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendToAll(string message)
        {
            _logger.LogInformation($"Broadcasting to all: {message}");
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        public async Task SendToUserAsync(string userId, string message)
        {
            _logger.LogInformation($"Sending to user {userId}: {message}");
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
        public async Task SendChatMessage(string fromUserId, string ticketId, string message)
        {
            // Get all users who should receive this message (ticket participants)
            var admins = await _IUser.GetAllAdminsAsync();
            var ticket = await _ITicket.GetTicketByIdAsync(Convert.ToInt32(ticketId));

            // Send to each participant except the sender
            foreach (var admin in admins.Where(u => u.UID.ToString() != fromUserId))
            {
                await Clients.User(admin.UID.ToString()).SendAsync("ReceiveChatMessage", fromUserId, message);
            }
            if (ticket.UID != 0 && ticket.UID.ToString() != fromUserId)
            {
                await Clients.User(ticket.UID.ToString()).SendAsync("ReceiveChatMessage", fromUserId, message);


            }
            if (ticket.AgentID != 0 && ticket.AgentID.ToString() != fromUserId)
            {
                await Clients.User(ticket?.AgentID?.ToString()).SendAsync("ReceiveChatMessage", fromUserId, message);
            }
        }

        public class UserInfo
        {
            public string? ConnectionId { get; set; }
            public string? UserId { get; set; }
            public Dictionary<string, string>? Claims { get; set; }
            public DateTime ConnectionTime { get; set; } = DateTime.Now;
        }
    }
}
