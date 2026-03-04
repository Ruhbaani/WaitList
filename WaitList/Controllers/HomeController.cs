using Microsoft.AspNetCore.Mvc;

namespace WaitList.API
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string  GetHello()
        {
            return "Hello";
        }

    }
}
