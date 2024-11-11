using Microsoft.Azure.Cosmos.Table;
using System.ComponentModel.DataAnnotations;

namespace ST10339549_CLDV6212_POE.Models
{
    public class Product : TableEntity
    {
        [Key]
        public string? ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? ProductDescription { get; set; }
        [Required]
        public double ProductPrice { get; set; }
    }
}
