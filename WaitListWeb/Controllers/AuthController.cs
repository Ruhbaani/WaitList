using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WaitListWeb.Models;

namespace WaitListWeb.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("/auth/login")]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost("/auth/login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginPost(string email, string password, string? returnUrl = null)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            ModelState.AddModelError("", "Invalid login.");
            return View("Login");
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid login.");
            return View("Login");
        }

        return Redirect(returnUrl ?? "/");
    }

    [HttpPost("/auth/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("/auth/denied")]
    public IActionResult Denied() => View();
}