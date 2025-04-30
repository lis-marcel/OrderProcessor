using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Service.DTO;
using System.Reflection;

namespace OrderProcessor.Service
{
    public static class OrderUtility
    {
        #region Public Methods

        public static OrderData? CreateOrderDetails(OrderCreationData orderCreationData)
        {
            return new()
            {
                ProductName = orderCreationData.ProductName,
                Value = orderCreationData.Value,
                Quantity = orderCreationData.Quantity,
                ShippingAddress = orderCreationData.ShippingAddress,
                CustomerId = orderCreationData.CustomerId,
                PaymentMethod = orderCreationData.PaymentMethod,
                CreationTime = DateTime.Now,
                Status = OrderStatus.New
            };
        }

        #endregion


    }
}