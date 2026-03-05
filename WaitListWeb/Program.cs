using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WaitListWeb.Data;
using WaitListWeb.Models;
using WaitListWeb.Security;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Db + Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Put AccountId into claims
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsPrincipalFactory>();

// Authorization: Roles + Tenant policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(TenantPolicies.SameAccount, policy =>
        policy.Requirements.Add(new SameAccountRequirement()));
});

builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, SameAccountHandler>();

// Cookie auth paths (so [Authorize] redirects to login)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/auth/denied";
});

var app = builder.Build();

// Seed roles + SystemAdmin user
await IdentitySeed.SeedAsync(app.Services, app.Configuration);

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();