using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.DTO
{
    public class OrderData
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

        public static Order ToBO(OrderData order)
        {
            return new Order
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

        public static OrderData ToDTO(Order order)
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
