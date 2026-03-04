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


        [HttpPost(Name = "CreateUser")]
        public async Task<bool> CreateUser(Users user)
        {

            _userService.CreateUser(user);
        }

        [HttpPost(Name = "AddUser")]
        public async Task<bool> AddUser(Users user)
        {

            _userService.AddUser(user);
        }

    }
}
