using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;
using System.Text.RegularExpressions;

public interface INotificationService
{
    Task SendChatMessageAsync(string fromUserId, string toUserId, string message);
    event Func<string, string, Task>? OnChatMessageReceived;
    Task InitializeAsync(Func<string, Task> onMessageReceived);
    Task SendToAllAsync(string message);
    Task SendToUserAsync(string userId, string message);
    ValueTask DisposeAsync();
    HubConnectionState ConnectionState { get; }
    Task AddToGroupAsync(string groupName);
    Task SendToGroupAsync(string groupName, string message);
    Task RemoveFromGroupAsync(string groupName);
}

public class NotificationService : INotificationService, IAsyncDisposable
{
    private readonly NavigationManager _navigationManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<NotificationService> _logger;
    private HubConnection? _hubConnection;
    private bool _disposed;
    private Func<string, Task>? _messageHandler;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly SemaphoreSlim _initLock = new(1, 1);
    public event Func<string, string, Task>? OnChatMessageReceived;

    public HubConnectionState ConnectionState => _hubConnection?.State ?? HubConnectionState.Disconnected;

    public NotificationService(
        NavigationManager navigationManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<NotificationService> logger)
    {
        _navigationManager = navigationManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }


    public async Task InitializeAsync(Func<string, Task> onMessageReceived)
    {
        if (_disposed) return;

        await _initLock.WaitAsync();
        try
        {
            if (_hubConnection is not null) return;

            _messageHandler = onMessageReceived;

            var authCookie = _httpContextAccessor.HttpContext?.Request.Cookies["loginCookie"];
            if (string.IsNullOrEmpty(authCookie))
                throw new InvalidOperationException("Authentication cookie is missing.");

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri("/notificationHub"), options =>
                {
                    options.Cookies = new CookieContainer();
                    options.Cookies.Add(new Cookie("loginCookie", authCookie, "/", new Uri(_navigationManager.BaseUri).Host));
                    options.Transports = HttpTransportType.WebSockets;
                })
                .WithAutomaticReconnect(new[] {
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
                })
                .AddMessagePackProtocol()
                .Build();

            _hubConnection.Closed += async (error) =>
            {
                _logger.LogWarning(error, "SignalR connection closed");
                await Task.Delay(new Random().Next(0, 5) * 1000);
            };

            _hubConnection.Reconnected += (connectionId) =>
            {
                _logger.LogInformation("SignalR reconnected with ID: {ConnectionId}", connectionId);
                return Task.CompletedTask;
            };

            _hubConnection.On<string>("ReceiveNotification", async json =>
            {
                if (_messageHandler is not null)
                    await _messageHandler.Invoke(json);
            });

            _hubConnection.On<string, string>("ReceiveChatMessage", async (fromUserId, message) =>
            {
                if (OnChatMessageReceived is not null)
                    await OnChatMessageReceived.Invoke(fromUserId, message);
            });

            await _hubConnection.StartAsync();
            _logger.LogInformation("SignalR connection started and NotificationService initialized");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize NotificationService");
            throw;
        }
        finally
        {
            _initLock.Release();
        }
    }




    public async Task SendToAllAsync(string message)
    {
        await _hubConnection!.SendAsync("SendToAll", message);
    }

    public async Task SendToUserAsync(string userId, string message)
    {
        await _hubConnection!.SendAsync("SendToUserAsync", userId, message);
    }

    public async Task SendChatMessageAsync(string fromUserId, string ticketId, string message)
    {
        await _hubConnection!.SendAsync("SendChatMessage", fromUserId, ticketId, message);
    }

    public async Task AddToGroupAsync(string groupName)
    {
        if (_hubConnection?.State != HubConnectionState.Connected)
            throw new InvalidOperationException("SignalR connection not active. Cannot add to group.");

        await _hubConnection.SendAsync("AddToGroup", groupName);
    }

    public async Task RemoveFromGroupAsync(string groupName)
    {
        await _hubConnection!.SendAsync("RemoveFromGroup", groupName);
    }

    public async Task SendToGroupAsync(string groupName, string message)
    {
        _hubConnection!.On<string, string>("ReceiveGroupMessage", async (fromUserId, message) =>
        {
            if (OnChatMessageReceived is not null)
                await OnChatMessageReceived.Invoke(fromUserId, message);
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        if (_hubConnection is not null)
        {
            try
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while disposing NotificationService");
            }
            _hubConnection = null;
        }

        _initLock.Dispose();
        _connectionLock.Dispose();
        _logger.LogInformation("NotificationService disposed");
    }
}