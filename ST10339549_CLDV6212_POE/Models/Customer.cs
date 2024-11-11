using Microsoft.Azure.Cosmos.Table;
using System.ComponentModel.DataAnnotations;

namespace ST10339549_CLDV6212_POE.Models
{
    public class Customer : TableEntity
    {
        [Key]
        public string? CustomerId { get; set; }

        [Required]
        public string? CustomerName { get; set; }

        [Required]
        public string? CustomerSurname { get; set; }

        [Required]
        [EmailAddress]
        public string? CustomerEmail { get; set; }

        [Required]
        public string? CustomerAddress { get; set; }
    }
}
