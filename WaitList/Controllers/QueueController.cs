using Microsoft.AspNetCore.Mvc;

namespace WaitList.API
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly ILogger<QueueController> _logger;

        public QueueController(ILogger<QueueController> logger)
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
