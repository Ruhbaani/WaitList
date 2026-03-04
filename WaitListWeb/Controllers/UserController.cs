using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WaitListWeb.Models;

namespace WaitListWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
       

        
    }
}
