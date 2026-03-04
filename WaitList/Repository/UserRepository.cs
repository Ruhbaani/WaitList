using Microsoft.EntityFrameworkCore;
using WaitList.DataAccess;

namespace WaitList.API
{
    public class UserRepository : IUserRepository
    {
        WaitListContext _dbcontext;
        public UserRepository()
        {
            _dbcontext = new WaitListContext();
        }

        //public List<Users> GetUserList()
        //{
        //    var context = new WaitListContext();
        //    List<Users> userList = new List<Users>();
        //    Users user;
        //    foreach (var dbuser in context.Users.ToList()){

        //        user = new Users();
        //        user.FirstName = dbuser.FirstName;
        //        user.AccountID =dbuser.AccountId;
        //        user.Email = dbuser.Email;
        //        userList.Add(user);
        //    }            
        //    return userList;               

        //}
        public async Task<List<Users>> GetUserList()
        {
          
            List<Users> userList = new List<Users>();
            Users user;
            foreach (var dbuser in _dbcontext.Users.ToList())
            {

                user = new Users();
                user.FirstName = dbuser.FirstName;
                user.AccountID = dbuser.AccountId;
                user.Email = dbuser.Email;
                userList.Add(user);
            }
            return userList;
        }
        public async Task<bool> CreateUser(Users user)
        {
            // context = new WaitListContext(); //context.Users US = new context.Users();
            WaitList.DataAccess.User u1 = new User();
            u1.FirstName = user.FirstName;
            u1.LastName = user.LastName;
            u1.Phone = user.Phone;
            u1.AccountId = user.AccountID;
            u1.UserName = user.Username;
            u1.Email = user.Email;
            _dbcontext.Users.Add(u1);
            _dbcontext.SaveChanges();
            return true;

        }


    }
}
