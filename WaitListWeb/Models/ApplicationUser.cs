using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WaitListWeb.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public int AccountId { get; set; }
}