using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AlexSupport.Services.Extensions
{
    public class LogService : ILogService
    {
        private readonly ITicketLogsHistoryRepository _logRepository;
        private readonly IAppUserRepoistory _appUserRepository;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILogger<LogService> _logger;
        private readonly ISystemLogsRepository _systemLogsRepository;

        public LogService(ITicketLogsHistoryRepository logRepository, IAppUserRepoistory appUserRepository, AuthenticationStateProvider authenticationStateProvider, ILogger<LogService> logger, ISystemLogsRepository systemLogsRepository)
        {
            _logRepository = logRepository;
            _appUserRepository = appUserRepository;
            _authenticationStateProvider = authenticationStateProvider;
            _logger = logger;
            _systemLogsRepository = systemLogsRepository;
        }

        public async Task CreateLogAsync(int TID, string Action)
        {
            try
            {
                // Get the current authenticated user
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                // Check if user is authenticated
                if (!user.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("Attempt to create log by unauthenticated user");
                    return;
                }

                // Get user ID claim
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    _logger.LogError("NameIdentifier claim not found for authenticated user");
                    return;
                }

                // Try to parse user ID
                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    _logger.LogError($"Invalid user ID format: {userIdClaim.Value}");
                    return;
                }

                // Get user from repository
                var appUser = await _appUserRepository.GetUserByIdAsync(userId);
                if (appUser == null)
                {
                    _logger.LogError($"User not found with ID: {userId}");
                    return;
                }

                // Create and save log
                var log = new Tlog
                {
                    TID = TID,
                    Action = Action,
                    actionTime = DateTime.Now,
                    UID = appUser.UID,
                };

                await _logRepository.CreateLog(log);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateLogAsync: {ex.Message}", ex);
                // Consider whether you want to throw or handle silently based on your requirements
            }
        }
        public async Task CreateSystemLogAsync(string Action, string type, int UID = 0)
        {
            try
            {
                // Get the current authenticated user
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                int userId = 0;
                if (UID != 0)
                {
                    userId = UID;
                }
                else
                {
                    // Check if user is authenticated
                    if (!user.Identity.IsAuthenticated)
                    {
                        _logger.LogWarning("Attempt to create log by unauthenticated user");
                        return;
                    }

                    // Get user ID claim
                    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim == null)
                    {
                        _logger.LogError("NameIdentifier claim not found for authenticated user");
                        return;
                    }

                    // Try to parse user ID
                    if (!int.TryParse(userIdClaim.Value, out int userid))
                    {
                        _logger.LogError($"Invalid user ID format: {userIdClaim.Value}");
                        return;
                    }
                    userId = Convert.ToInt32(userid);
                }

                // Get user from repository
                var appUser = await _appUserRepository.GetUserByIdAsync(userId);
                if (appUser == null)
                {
                    _logger.LogError($"User not found with ID: {userId}");
                    return;
                }

                // Create and save log
                var log = new SystemLogs
                {
                    Action = Action,
                    actionTime = DateTime.Now,
                    UID = appUser.UID,
                    Type = type,
                };

                await _systemLogsRepository.CreateLog(log);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateLogAsync: {ex.Message}", ex);
            }
        }
        public async Task<int> ReturnCurrentUserID()
        {
            try
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }

                _logger.LogWarning("User ID claim not found or not an integer");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReturnCurrentUserID: {ex.Message}", ex);
                return 0;
            }
        }

        public async Task<string> ReturnCurrentUserRole()
        {
            try
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user?.Identity?.IsAuthenticated != true)
                {
                    _logger.LogWarning("User is not authenticated");
                    return string.Empty;
                }

                var roleClaim = user.FindFirst(ClaimTypes.Role) ?? user.FindFirst("role");

                if (roleClaim == null)
                {
                    _logger.LogWarning("Role claim not found for authenticated user");
                    return string.Empty;
                }

                return roleClaim.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current user role");
                return string.Empty;
            }
        }
    }


}
