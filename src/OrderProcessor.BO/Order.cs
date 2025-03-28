using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.BO
{
    public class Order
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public required string ProductName { get; set; }
        public required string ShippingAddress { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? MarkToShippingAt { get; set; }
        public OrderStatus Status { get; set; }
        public CustomerType CustomerType { get; set; }
        public required string CustomerName { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}