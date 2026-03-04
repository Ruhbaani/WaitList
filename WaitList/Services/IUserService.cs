using System;
namespace WaitList.API
{
    public interface IUserService
    {
        Task<List<Users>> GetUserList();

        Task<bool> CreateUser(Users user);

        Task<bool> AddUser(Users user);

    }
}
