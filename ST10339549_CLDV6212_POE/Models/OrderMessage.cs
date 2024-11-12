using System.ComponentModel.DataAnnotations;

namespace ST10339549_CLDV6212_POE.Models
{
    public class OrderMessage
    {
        [Key]
        public string? OrderId { get; set; }
        [Required]
        public string? ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string Action { get; set; } = "In Progress";
        public DateTimeOffset? Timestamp { get; set; }
    }
}
