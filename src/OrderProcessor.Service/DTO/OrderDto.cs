using OrderProcessor.BO.Entities;
using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.Service.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public required string ProductName { get; set; }
        public required string ShippingAddress { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? MarkToShippingAt { get; set; }
        public OrderStatus Status { get; set; }
        public int CustomerId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        #region Public Methods
        public static Order ToBo(OrderDto order)
        {
            return new Order
            {
                Id = order.Id,
                Value = order.Value,
                ProductName = order.ProductName,
                ShippingAddress = order.ShippingAddress,
                Quantity = order.Quantity,
                CreationTime = order.CreationTime,
                MarkToShippingAt = order.MarkToShippingAt,
                Status = order.Status,
                CustomerId = order.CustomerId,
                PaymentMethod = order.PaymentMethod
            };
        }

        public static OrderDto ToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Value = order.Value,
                ProductName = order.ProductName,
                ShippingAddress = order.ShippingAddress,
                Quantity = order.Quantity,
                CreationTime = order.CreationTime,
                MarkToShippingAt = order.MarkToShippingAt,
                Status = order.Status,
                CustomerId = order.CustomerId,
                PaymentMethod = order.PaymentMethod
            };
        }
        #endregion

    }
}
