using OrderProcessor.BO.Entities;
using OrderProcessor.BO.OrderOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime LastLoginAt { get; set; }
        public CustomerType CustomerType { get; set; }
        public AccountType AccountType { get; set; }

        public static User ToBo(UserDto customerData)
        {
            return new User
            {
                Id = customerData.Id,
                Name = customerData.Name,
                Email = customerData.Email,
                Password = customerData.Password,
                LastLoginAt = customerData.LastLoginAt,
                CustomerType = customerData.CustomerType,
                AccountType = customerData.AccountType,
            };
        }

        public static UserDto ToDto(User customer)
        {
            return new UserDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Password = customer.Password,
                LastLoginAt = customer.LastLoginAt,
                CustomerType = (CustomerType)customer.CustomerType!,
                AccountType= customer.AccountType,
            };
        }

    }
}
