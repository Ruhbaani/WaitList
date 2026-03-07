using System.ComponentModel.DataAnnotations;

namespace WaitListWeb.Models
    {
    public class QueueEntry
    {
        [Key]
        public int QueueEntryId { get; set; } = 0;

        public int AccountId { get; set; } = 0;
        public int QueueId { get; set; } = 0;
        public int? ServiceId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsNotified { get; set; } = false;
        public bool IsServed { get; set; } = false;
        public int Position { get; set; } = 0;
    }
}