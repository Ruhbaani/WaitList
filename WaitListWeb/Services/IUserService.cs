using System;
using WaitListWeb.Models;
namespace WaitListWeb.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetUserList();


        Task<bool> CreateUser(ApplicationUser user);

    }
}
