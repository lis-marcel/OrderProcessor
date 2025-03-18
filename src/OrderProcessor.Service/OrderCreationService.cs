using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public static class OrderCreationService
    {
        public static void CreateOrder(DbStorage dbStorageContext, MessageLogger logger)
        {
            try
            {
                var orderData = OrderUtility.CreateOrderDetails(logger);
                if (orderData == null)
                {
                    logger.WriteWarning("Order creation cancelled.");
                    return;
                }

                int orderId = DbStorageService.GetHighestOrderId(dbStorageContext) + 1;
                orderData.Id = orderId;

                dbStorageContext.Orders.Add(OrderData.ToBO(orderData));
                dbStorageContext.SaveChanges();
                logger.WriteSuccess($"Order created successfully with ID: {orderId}");
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }

    }
}