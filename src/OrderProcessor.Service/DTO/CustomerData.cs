using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.DTO
{
    public class CustomerData
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required CustomerType CustomerType { get; set; }

        public static Customer ToBO(CustomerData customerData)
        {
            return new Customer
            {
                Id = customerData.Id,
                Name = customerData.Name,
                CustomerType = customerData.CustomerType
            };
        }

        public static CustomerData ToDTO(Customer customer)
        {
            return new CustomerData
            {
                Id = customer.Id,
                Name = customer.Name,
                CustomerType = customer.CustomerType
            };
        }

    }
}
