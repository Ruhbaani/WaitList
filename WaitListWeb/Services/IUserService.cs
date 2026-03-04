using System;
using WaitListWeb.Models;
namespace WaitListWeb.Services
{
    public interface IUserService
    {
        Task<List<Users>> GetUserList();


        Task<bool> CreateUser(Users user);

    }
}
