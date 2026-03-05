using Microsoft.AspNetCore.Authorization;

namespace WaitListWeb.Security;

public static class TenantPolicies
{
    public const string SameAccount = "SameAccount";
}

public class SameAccountRequirement : IAuthorizationRequirement { }