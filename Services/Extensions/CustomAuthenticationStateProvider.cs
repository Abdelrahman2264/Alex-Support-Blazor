using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger <CustomAuthenticationStateProvider> logger;

    public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor, ILogger<CustomAuthenticationStateProvider> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        this.logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(user);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in Get Authentication State: {ex.Message}", ex);
            return new AuthenticationState(new ClaimsPrincipal());
        }
      ;
    }

    public async Task SignInAsync(ClaimsPrincipal user)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        catch (Exception ex)
        {
            logger.LogError($"Error In Sign In : {ex.Message}", ex);

        }
    }

    public async Task SignOutAsync()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        catch (Exception ex)
        {
            logger.LogError($"Error In Sign Out : {ex.Message}", ex);

        }
    }
}