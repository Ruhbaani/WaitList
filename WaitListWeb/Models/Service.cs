using System.ComponentModel.DataAnnotations;

namespace WaitListWeb.Models
{
    public class Service
    {
        public Service() { }

        [Key]
        public int ServiceID { get; set; } = 0;

        public string ServiceType { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;

    }
}
