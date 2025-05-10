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
    public class CustomerData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime LastLoginAt { get; set; }
        public Guid SessionToken { get; set; }
        public CustomerType CustomerType { get; set; }

        public static Customer ToBO(CustomerData customerData)
        {
            return new Customer
            {
                Id = customerData.Id,
                Name = customerData.Name,
                Email = customerData.Email,
                Password = customerData.Password,
                LastLoginAt = customerData.LastLoginAt,
                CustomerType = customerData.CustomerType
            };
        }

        public static CustomerData ToDTO(Customer customer)
        {
            return new CustomerData
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Password = customer.Password,
                LastLoginAt = customer.LastLoginAt,
                CustomerType = customer.CustomerType
            };
        }

    }
}
