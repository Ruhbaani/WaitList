using System.ComponentModel.DataAnnotations;

namespace WaitListWeb.Models
{
    public class Queue
    {
        public Queue() { }

        [Key]
        public int QueueID { get; set; } = 0;

        public int CustomerID { get; set; } = 0;
        public int AccountID { get; set; } = 0;
        public string DateTime { get; set; } = string.Empty;
        public int ServiceID { get; set; } = 0;


    }
}
