using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using OrderProcessor.Common;
using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.Service
{
    public static class OrderStatusService
    {
        #region Public Methods
        public static void ChangeStatus(DbStorage dbStorageContext, ConsoleLogger logger)
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
                orderData.Status = (Status)OrderUtility.GetValidOrderStatus(logger);
                order.Status = orderData.Status;

                dbStorageContext.SaveChanges();
                logger.WriteSuccess("Order status changed successfully.");
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }
        #endregion

    }
}