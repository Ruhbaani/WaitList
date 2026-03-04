using Microsoft.AspNetCore.Mvc;
using WaitList.API;

namespace WaitList.API
{
    public class UsersService:IUserService
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        public UsersService(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;

        }


        public async Task<List<Users>> GetUserList()
        {
           
            return await _userRepository.GetUserList();


        }
        
        public async Task<bool> CreateUser(Users user)
        {
            UserRepository UP = new UserRepository();
            UP.CreateUser(user);
            return true;
        }

        public async Task<bool> AddUser(Users user)
        {
            UserRepository UP = new UserRepository();
            UP.AddUser(user);
            return true;
        }
    }
}
