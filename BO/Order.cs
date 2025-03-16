using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderProcessor.OrderOptions;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.BO
{
    class Order
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public string ProductName { get; set; }
        public string ShippingAddress { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
        public Status Status { get; set; }
        public CustomerType CustomerType { get; set; }
        public string CustomerName { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public static OrderData OrderToOrderData(Order order)
        {
            return new OrderData
            {
                Id = order.Id,
                Value = order.Value,
                ProductName = order.ProductName,
                ShippingAddress = order.ShippingAddress,
                Quantity = order.Quantity,
                CreationTime = order.CreationTime,
                Status = order.Status,
                CustomerType = order.CustomerType,
                CustomerName = order.CustomerName,
                PaymentMethod = order.PaymentMethod
            };
        }

    }
}