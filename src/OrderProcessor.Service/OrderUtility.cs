using KellermanSoftware.CompareNetObjects;
using OrderProcessor.BO.Entities;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public static class OrderUtility
    {
        #region Public Methods

        public static OrderDto CreateOrderDetails(OrderCreationDto orderCreationData)
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

        public static bool OrdersDiffer(object retrievedOrder, object updatedOrderDto)
        {
            if (retrievedOrder == null || updatedOrderDto == null)
                return true;

            // Handle Order to OrderDto comparison (most common case for editing)
            if (retrievedOrder is Order order && updatedOrderDto is OrderDto dto)
            {
                return order.Value != dto.Value ||
                       order.ProductName != dto.ProductName ||
                       order.ShippingAddress != dto.ShippingAddress ||
                       order.Quantity != dto.Quantity ||
                       order.MarkToShippingAt != dto.MarkToShippingAt ||
                       order.Status != dto.Status ||
                       order.CustomerId != dto.CustomerId ||
                       order.PaymentMethod != dto.PaymentMethod;
            }

            // Handle OrderDto to Order comparison
            if (retrievedOrder is OrderDto dto1 && updatedOrderDto is Order order1)
            {
                return dto1.Value != order1.Value ||
                       dto1.ProductName != order1.ProductName ||
                       dto1.ShippingAddress != order1.ShippingAddress ||
                       dto1.Quantity != order1.Quantity ||
                       dto1.MarkToShippingAt != order1.MarkToShippingAt ||
                       dto1.Status != order1.Status ||
                       dto1.CustomerId != order1.CustomerId ||
                       dto1.PaymentMethod != order1.PaymentMethod;
            }

            // Handle Order to Order comparison
            if (retrievedOrder is Order dbOrder && updatedOrderDto is Order updateOrder)
            {
                return dbOrder.Value != updateOrder.Value ||
                       dbOrder.ProductName != updateOrder.ProductName ||
                       dbOrder.ShippingAddress != updateOrder.ShippingAddress ||
                       dbOrder.Quantity != updateOrder.Quantity ||
                       dbOrder.MarkToShippingAt != updateOrder.MarkToShippingAt ||
                       dbOrder.Status != updateOrder.Status ||
                       dbOrder.CustomerId != updateOrder.CustomerId ||
                       dbOrder.PaymentMethod != updateOrder.PaymentMethod;
            }

            return false; // If types don't match expected patterns, assume no differences
        }

    }

    #endregion

}