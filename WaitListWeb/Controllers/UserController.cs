using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WaitListWeb.Models;
using WaitListWeb.Security;

namespace WaitListWeb.Controllers;

[Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner}")]
[Authorize(Policy = TenantPolicies.SameAccount)]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // Example: list users in your same AccountId (tenant)
    public IActionResult Index()
    {
        // NOTE: For tenant scoping you’ll filter by AccountId. For this we need the claim.
        var accountId = User.FindFirst(ApplicationClaimsPrincipalFactory.AccountIdClaimType)?.Value ?? "0";

        // If SystemAdmin you might return all; otherwise filter to this account.
        var users = _userManager.Users
            .Where(u => User.IsInRole(AppRoles.SystemAdmin) || u.AccountId == accountId)
            .ToList();

        return View(users);
    }
}