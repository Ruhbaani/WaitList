using Microsoft.AspNetCore.Mvc;

namespace WaitList.API
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }
        /*public List<Users> GetUserList()
        {
            UserRepository UP = new UserRepository();
            return UP.GetUserList();
        }*/

    }
}


