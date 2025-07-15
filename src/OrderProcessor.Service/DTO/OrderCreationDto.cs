using OrderProcessor.BO.OrderOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.DTO
{
    public class OrderCreationDto
    {
        public double Value { get; set; }
        public required string ProductName { get; set; }
        public required string ShippingAddress { get; set; }
        public int Quantity { get; set; }
        public int CustomerId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
