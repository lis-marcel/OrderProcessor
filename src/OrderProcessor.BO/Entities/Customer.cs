using OrderProcessor.BO.OrderOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.BO.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public DateTime LastLoginAt { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
