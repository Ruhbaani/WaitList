namespace WaitList.API
{
    public interface IUserRepository
    {
        Task<List<Users>> GetUserList();

        Task<bool> CreateUser(Users user);


    }
}
