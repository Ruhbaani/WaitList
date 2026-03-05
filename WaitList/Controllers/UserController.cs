using Microsoft.AspNetCore.Mvc;
using WaitListAPI.Services;

namespace WaitList.API
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;

        }

        //public AsyncUserController(IUserRepository userRepository)
        //{
        //    _userRepository = userRepository;
        //}

        //[HttpGet(Name = "GetUserList")]
        //public List<Users> GetUserList()
        //{
        //    UsersService usersService = new UsersService();
        //    return usersService.GetUserList();
        //}

        [HttpGet(nameof(GetUserList))]
        public async Task<List<Users>> GetUserList()
        {
           return await _userService.GetUserList();
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Users user)
        {
            var success = await _userService.CreateUser(user);

            if (!success)
                return BadRequest("Could not create user.");

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Users user)
        {
            // await your service call
            var success = await _userService.AddUser(user);

            if (!success)
                return BadRequest("Could not add user.");

            return Ok();
        }

    }
}
