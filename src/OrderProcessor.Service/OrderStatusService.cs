using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using OrderProcessor.Common;
using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.Service
{
    public class OrderStatusService
    {
        #region Public Methods
        public static void ChangeStatus(DbStorage dbStorageContext, ConsoleLogger consoleLogger)
        {
            try
            {
                var order = OrderUtility.AskAndFindOrder(dbStorageContext, consoleLogger);

                if (order == null)
                {
                    consoleLogger.WriteInfo("Order not found.");
                    return;
                }

                var orderData = OrderData.ToDTO(order);
                orderData.Status = (OrderStatus)OrderUtility.GetValidOrderStatus(consoleLogger);
                order.Status = orderData.Status;

                dbStorageContext.SaveChanges();
                consoleLogger.WriteSuccess("Order status changed successfully.");
            }
            catch (Exception ex)
            {
                consoleLogger.WriteError(ex.Message);
            }
        }

        public static async Task ProcessPendingOrders(DbStorage dbStorageContext, ConsoleLogger consoleLogger)
        {
            consoleLogger.WriteInfo("Processing pending orders...");

            try
            {
                var now = DateTime.Now.AddMilliseconds(-4500);
                var pendingOrders = dbStorageContext.Orders.Where(o =>
                    o.Status == OrderStatus.PendingToShipping &&
                    o.MarkToShippingAt <= now);

                if (!pendingOrders.Any())
                {
                    consoleLogger.WriteInfo("No pending orders found.");
                    return;
                }

                foreach (var order in pendingOrders)
                {
                    var orderData = OrderData.ToDTO(order);
                    orderData.Status = OrderStatus.InShipping;

                    order.Status = orderData.Status;
                }

                consoleLogger.WriteSuccess("Pending orders processed successfully.");

                await dbStorageContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                consoleLogger.WriteError(ex.Message);
                consoleLogger.WriteError("Failed to process pending orders.");
            }
        }

        #endregion

    }
}