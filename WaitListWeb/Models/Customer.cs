using System.ComponentModel.DataAnnotations;

namespace WaitListWeb.Models
{
    public class Customer
    {

        public Customer() 
        { 
        }

        [Key]
        public int CustomerId { get; set; } = 0;

        public string FirstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int AccountID { get; set; } = 0;


    }

}
