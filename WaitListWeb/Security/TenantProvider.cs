using System.Security.Claims;

namespace WaitListWeb.Security;

public interface ITenantProvider
{
    string? GetAccountId();
    bool IsSystemAdmin();
}

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _http;

    public TenantProvider(IHttpContextAccessor http) => _http = http;

    public string? GetAccountId()
        => _http.HttpContext?.User?.FindFirstValue(ApplicationClaimsPrincipalFactory.AccountIdClaimType);

    public bool IsSystemAdmin()
        => _http.HttpContext?.User?.IsInRole(AppRoles.SystemAdmin) == true;
}