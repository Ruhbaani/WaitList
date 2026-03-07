using System.ComponentModel.DataAnnotations;

namespace WaitListWeb.Models
{
    public class Account
    {
        public Account() { }

        [Key]
        public int AccountId { get; set; } = 0;

        public string OrgName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ProvinceId { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

    }
}
