using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.Service.DTO
{
    public class OrderData
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public required string ProductName { get; set; }
        public required string ShippingAddress { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
        public Status Status { get; set; }
        public CustomerType CustomerType { get; set; }
        public required string CustomerName { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        #region Public Methods
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
        #endregion

    }
}
