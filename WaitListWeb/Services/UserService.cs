using Microsoft.AspNetCore.Mvc;
using WaitListWeb.Models;
using System;
namespace WaitListWeb.Services
{
    public class UsersService : IUserService
    {
        private readonly ILogger<UsersService> _logger;
        public UsersService(ILogger<UsersService> logger)
        {
            _logger = logger;
            

        }

        public Task<List<ApplicationUser>> GetUserList()
        {
            // TODO: replace with real implementation
            return Task.FromResult(new List<ApplicationUser>());
        }

        public Task<bool> CreateUser(ApplicationUser user)
        {
            // TODO: replace with real implementation
            return Task.FromResult(true);
        }

        //public async Task<List<Users>> GetUserList()
        //{

        //    return await _userRepository.GetUserList();


        //}

        //public async Task<bool> CreateUser(Users user)
        //{

        //    return true;
        //}
    }
}
