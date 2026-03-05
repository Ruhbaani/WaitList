namespace WaitListWeb.Security
{
    public class AppRoles
    {
        public const string SystemAdmin = "SystemAdmin";    
        public const string AccountOwner = "AccountOwner";
        public const string Manager = "Manager";
        public const string Host = "Host";
        public const string Staff = "Staff";
        public const string ReadOnly = "ReadOnly";

        public static readonly string[] All =
        [
            SystemAdmin, 
            AccountOwner, 
            Manager, 
            Host, 
            Staff, 
            ReadOnly
        ];
    }
}
