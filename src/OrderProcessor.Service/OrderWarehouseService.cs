using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using OrderProcessor.Common;
using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.Console.Service
{
    public static class WarehouseService
    {
        #region Public Methods
        public static void MoveToWarehouse(DbStorage dbStorageContext, ConsoleLogger logger)
        {
            try
            {
                var order = OrderUtility.AskAndFindOrder(dbStorageContext, logger);
                if (order == null) 
                {
                    logger.WriteInfo("Order not found.");
                    return; 
                }

                var orderData = OrderData.ToDTO(order);
                bool isEligible = OrderBusinessLogic.IsOrderEligibleForWarehouseProcessing(orderData);

                if (!isEligible)
                {
                    logger.WriteWarning("Order is not eligible for moving to warehouse.\nReturning order to customer.");
                    orderData.Status = OrderStatus.ReturnedToCustomer;
                }
                else
                {
                    orderData.Status = OrderStatus.InWarehouse;
                    logger.WriteSuccess("Order moved to warehouse successfully.");
                }

                order.Status = orderData.Status;

                dbStorageContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }

        public static void MoveToShipping(DbStorage dbStorageContext, ConsoleLogger logger)
        {
            try
            {
                var orderId = OrderUtility.EnterOrderId(logger);
                Task.Run(async () =>
                    await OrderBusinessLogic.MarkOrderAsToShippingAfterDelay(dbStorageContext, orderId)
                );
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }
        #endregion

    }
}
