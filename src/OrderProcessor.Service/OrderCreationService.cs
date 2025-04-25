using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using OrderProcessor.Common;
using OrderProcessor.Console.Service;

namespace OrderProcessor.Service
{
    public class OrderCreationService
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

                dbStorageContext.Orders.Add(OrderData.ToBO(orderData));
                dbStorageContext.SaveChanges();

                int newOrderId = DbStorageService.GetHighestId<Order>(dbStorageContext);

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