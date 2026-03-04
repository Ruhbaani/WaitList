using Microsoft.AspNetCore.Mvc;

namespace WaitList.API
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(ILogger<ServiceController> logger)
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
