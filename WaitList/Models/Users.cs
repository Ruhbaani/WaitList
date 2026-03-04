using System.Security.Cryptography.X509Certificates;

namespace WaitList.API
{
    public class Users
    {
        public Users()
        {

        }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int AccountID { get; set; } =0;
        public int UserID { get; set; } = 0;




    }
}
