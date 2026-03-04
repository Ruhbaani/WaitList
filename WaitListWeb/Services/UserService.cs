using Microsoft.AspNetCore.Mvc;
using WaitListWeb.Models;
using System;
namespace WaitListWeb.Services
{
    public class UsersService
    {
        private readonly ILogger<UsersService> _logger;
        public UsersService(ILogger<UsersService> logger)
        {
            _logger = logger;
            

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
