using Microsoft.AspNetCore.Identity;

namespace WaitListWeb.Models;

public class ApplicationUser : IdentityUser
{
    // Tenant / restaurant account scope
    // Your existing models already carry AccountID fields :contentReference[oaicite:4]{index=4}
    public string AccountId { get; set; } = "0";
}