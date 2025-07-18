﻿using OrderProcessor.BO.OrderOptions;
using System.ComponentModel.DataAnnotations;

namespace OrderProcessor.BO.Entities
{
    public class User
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
        public AccountType AccountType { get; set; }
        public CustomerType? CustomerType { get; set; }
    }
}
