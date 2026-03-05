using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WaitListWeb.Security;

public class SameAccountHandler : AuthorizationHandler<SameAccountRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SameAccountRequirement requirement)
    {
        // If SystemAdmin, skip tenant restrictions
        if (context.User.IsInRole(AppRoles.SystemAdmin))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Require account_id claim for everyone else
        var acct = context.User.FindFirstValue(ApplicationClaimsPrincipalFactory.AccountIdClaimType);
        if (!string.IsNullOrWhiteSpace(acct))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}