using Microsoft.AspNetCore.Mvc;

namespace WaitList.API
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
        /*
        [HttpGet]    
        public List<Users> GetUserList()
        {
            UserRepository UP = new UserRepository();
            return UP.GetUserList();
        }
        */
    }
}
