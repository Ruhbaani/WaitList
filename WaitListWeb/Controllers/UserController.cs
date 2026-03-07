using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WaitListWeb.Models;
using WaitListWeb.Security;

namespace WaitListWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{AppRoles.SystemAdmin},{AppRoles.AccountOwner}")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    private int? GetCurrentAccountId()
    {
        var claim = User.FindFirst(ApplicationClaimsPrincipalFactory.AccountIdClaimType)?.Value
                    ?? User.FindFirst("account_id")?.Value;

        return int.TryParse(claim, out var accountId) ? accountId : null;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var currentAccountId = GetCurrentAccountId();

        if (!User.IsInRole(AppRoles.SystemAdmin) && currentAccountId is null)
            return Unauthorized();

        var query = _userManager.Users.AsQueryable();

        if (!User.IsInRole(AppRoles.SystemAdmin))
            query = query.Where(u => u.AccountId == currentAccountId!.Value);

        var users = await query
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.UserName,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.AccountId
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var currentAccountId = GetCurrentAccountId();
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return NotFound();

        if (!User.IsInRole(AppRoles.SystemAdmin) && user.AccountId != currentAccountId)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            user.Id,
            user.Email,
            user.UserName,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.AccountId,
            Roles = roles
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentAccountId = GetCurrentAccountId();
        if (!User.IsInRole(AppRoles.SystemAdmin) && currentAccountId is null)
            return Unauthorized();

        var accountIdToUse = User.IsInRole(AppRoles.SystemAdmin)
            ? dto.AccountId
            : currentAccountId!.Value;

        if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            return BadRequest(new { message = "A user with that email already exists." });

        if (!await _roleManager.RoleExistsAsync(dto.Role))
            return BadRequest(new { message = $"Role '{dto.Role}' does not exist." });

        var user = new ApplicationUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.UserName,
            PhoneNumber = dto.PhoneNumber,
            AccountId = accountIdToUse
        };

        var createResult = await _userManager.CreateAsync(user, dto.Password);
        if (!createResult.Succeeded)
            return BadRequest(createResult.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);
        if (!roleResult.Succeeded)
            return BadRequest(roleResult.Errors);

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new
        {
            user.Id,
            user.Email,
            user.UserName,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.AccountId,
            Role = dto.Role
        });
    }
}