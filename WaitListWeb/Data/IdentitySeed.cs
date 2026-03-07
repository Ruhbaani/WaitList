using Microsoft.AspNetCore.Identity;
using WaitListWeb.Models;
using WaitListWeb.Security;

namespace WaitListWeb.Data;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration config)
    {
        using var scope = services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in AppRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var email = config["Seed:SystemAdminEmail"] ?? "admin@waitlist.local";
        var password = config["Seed:SystemAdminPassword"] ?? "ChangeMe!12345";
        var acctId = int.Parse(config["Seed:SystemAdminAccountId"] ?? "0");

        var admin = await userManager.FindByEmailAsync(email);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Admin",
                AccountId = acctId
            };

            var create = await userManager.CreateAsync(admin, password);
            if (!create.Succeeded)
            {
                var errors = string.Join("; ", create.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create SystemAdmin: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, AppRoles.SystemAdmin))
        {
            await userManager.AddToRoleAsync(admin, AppRoles.SystemAdmin);
        }
    }
}