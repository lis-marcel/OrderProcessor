using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public static class OrderCreationService
    {
        #region Public Methods
        public static void CreateOrder(DbStorage dbStorageContext, ConsoleLogger logger)
        {
            try
            {
                var orderData = OrderUtility.CreateOrderDetails(dbStorageContext, logger);

                if (orderData == null)
                {
                    logger.WriteInfo("Order creation cancelled.");
                    return;
                }

                int newOrderId = DbStorageService.GetHighestId<Order>(dbStorageContext) + 1;
                orderData.Id = newOrderId;

                dbStorageContext.Orders.Add(OrderData.ToBO(orderData));
                dbStorageContext.SaveChanges();
                logger.WriteSuccess($"Order created successfully with ID: {newOrderId}");
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }
        #endregion

    }
}