using OrderProcessor.BO.Entities;
using OrderProcessor.BO.OrderOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.DTO
{
    public class UserAccountDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime LastLoginAt { get; set; }
        public CustomerType CustomerType { get; set; }
        public AccountType AccountType { get; set; }

        public static User ToBo(UserAccountDto customerData)
        {
            return new User
            {
                Id = customerData.Id,
                Name = customerData.Name,
                Email = customerData.Email,
                LastLoginAt = customerData.LastLoginAt,
                CustomerType = customerData.CustomerType,
                AccountType = customerData.AccountType,
            };
        }

        public static UserAccountDto ToDto(User customer)
        {
            return new UserAccountDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                LastLoginAt = customer.LastLoginAt,
                CustomerType = (CustomerType)customer.CustomerType!,
                AccountType = customer.AccountType,
            };
        }

    }
}
