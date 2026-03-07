namespace WaitListWeb.Security
{
    public class AppRoles
    {
        public const string SystemAdmin = "SystemAdmin";    
        public const string AccountOwner = "AccountOwner";
        public const string Manager = "Manager";
        public const string Server = "Server";
        public const string ReadOnly = "ReadOnly";

        public static readonly string[] All =
        [
            SystemAdmin, 
            AccountOwner, 
            Manager, 
            Server,
            ReadOnly
        ];
    }
}
