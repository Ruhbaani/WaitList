using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaitListWeb.Data;
using WaitListWeb.Models;
using WaitListWeb.Security;

namespace WaitListWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.SystemAdmin)]
    public async Task<IActionResult> GetAccounts()
    {
        var accounts = await _context.Accounts.ToListAsync();
        return Ok(accounts);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetAccountById(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account is null)
            return NotFound();

        if (User.IsInRole(AppRoles.SystemAdmin))
            return Ok(account);

        var claim = User.FindFirst(ApplicationClaimsPrincipalFactory.AccountIdClaimType)?.Value
                    ?? User.FindFirst("account_id")?.Value;

        if (!int.TryParse(claim, out var currentAccountId))
            return Unauthorized();

        if (currentAccountId != id)
            return NotFound();

        return Ok(account);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _userManager.FindByEmailAsync(dto.OwnerEmail) is not null)
            return BadRequest(new { message = "A user with that owner email already exists." });

        var account = new Account
        {
            OrgName = dto.OrgName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            ProvinceId = dto.ProvinceId,
            ZipCode = dto.ZipCode
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        if (!await _roleManager.RoleExistsAsync(AppRoles.AccountOwner))
            await _roleManager.CreateAsync(new IdentityRole(AppRoles.AccountOwner));

        var ownerUser = new ApplicationUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.OwnerEmail,
            UserName = dto.OwnerUserName,
            PhoneNumber = dto.Phone,
            AccountId = account.AccountId
        };

        var createUserResult = await _userManager.CreateAsync(ownerUser, dto.OwnerPassword);
        if (!createUserResult.Succeeded)
            return BadRequest(createUserResult.Errors);

        var roleResult = await _userManager.AddToRoleAsync(ownerUser, AppRoles.AccountOwner);
        if (!roleResult.Succeeded)
            return BadRequest(roleResult.Errors);

        return CreatedAtAction(nameof(GetAccountById), new { id = account.AccountId }, new
        {
            account.AccountId,
            account.OrgName,
            owner = new
            {
                ownerUser.Id,
                ownerUser.Email,
                ownerUser.UserName,
                ownerUser.AccountId
            }
        });
    }
}