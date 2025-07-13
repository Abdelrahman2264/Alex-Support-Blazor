using AlexSupport.Components;
using AlexSupport.Data;
using AlexSupport.Hubs;
using AlexSupport.Repository;
using AlexSupport.Repository.IRepository;
using AlexSupport.Services.Extensions;
using AlexSupport.Services.Models;
using AlexSupport.ViewModels;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Configuration
// =========================

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// =========================
// Core Services
// =========================

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();



// Compression for SignalR
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "loginCookie";
        options.LoginPath = "/";
        options.AccessDeniedPath = "/AccessDenied";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(420);
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.Lax;

        // Critical for SignalR
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always in production
        options.Events = new CookieAuthenticationEvents
        {
            OnSigningIn = ctx =>
            {
                if (ctx.Principal?.Identity?.Name != null &&
                    !ctx.Principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, ctx.Principal.Identity.Name),
                        // Add any other required claims
                        new Claim("username", ctx.Principal.Identity.Name)
                    };
                    ctx.Principal.AddIdentity(new ClaimsIdentity(claims));
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.Always;
});
// =========================
// Blazor & SignalR Services
// =========================

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpClient();
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024); // 10MB
builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazorBootstrap();

// =========================
// App Services
// =========================

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IAppUserRepoistory, AppUserRepoistory>();
builder.Services.AddScoped<IDailyTaskRepository, DailyTasksRepository>();
builder.Services.AddScoped<ITicketLogsHistoryRepository, TicketLogsHistoryRepository>();
builder.Services.AddScoped<INotificationRepoisitory, NotificationRepository>();
builder.Services.AddScoped<EmailServices>();
builder.Services.AddScoped<ILogService, LogService>(); 
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<INotificationService, NotificationService>(); 
builder.Services.AddScoped<IChatMessageRepoisitory, ChatMessageRepository>(); 
builder.Services.AddScoped<ITicketChatService, TicketChatService>(); 
builder.Services.AddScoped<ISystemLogsRepository, SystemLogsRepository>();
builder.Services.AddHostedService<ScheduledBackgroundService>();
builder.Services.AddScoped(provider =>
{
    var navigationManager = provider.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
        .WithUrl(navigationManager.ToAbsoluteUri("/notificationHub"))
        .Build();
});

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
}).AddMessagePackProtocol();

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddSingleton<IPasswordHasherRepository, PasswordHasherRepository>(_ =>
    new PasswordHasherRepository(
        workFactor: 12,
        hashType: HashType.SHA256,
        enhancedEntropy: true));

// =========================
// Database Context
// =========================

builder.Services.AddDbContext<AlexSupportDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================
// Logging
// =========================

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// =========================
// Build and Run App
// =========================

var app = builder.Build();

// =========================
// Middleware Pipeline
// =========================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseAntiforgery();
app.UseStaticFiles();
//  FIXED ORDER (Auth before Authorization)
app.UseAuthentication();
app.UseAuthorization();

// SignalR Hub Mapping
app.MapHub<NotificationHub>("/notificationHub");

// Blazor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
