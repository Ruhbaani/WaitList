using System.ComponentModel.DataAnnotations;

namespace WaitListWeb.Models
{
    public class Queue
    {
        public Queue() { }

        [Key]
        public int QueueId { get; set; } = 0;

        public int AccountId { get; set; } = 0;

        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;  

    }
}
