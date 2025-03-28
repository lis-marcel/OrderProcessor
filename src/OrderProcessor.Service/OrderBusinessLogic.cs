using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public class OrderBusinessLogic
    {
        private static readonly double cashPaymentThreshold = 2500;
        private static readonly int updateDelay = 4500;
        private static readonly ConsoleLogger consoleLogger = new();

        public OrderBusinessLogic() { }

        #region Public Methods
        public static bool IsOrderEligibleForWarehouseProcessing(OrderData orderData)
        {
            return true && orderData.Value > cashPaymentThreshold && orderData.PaymentMethod == PaymentMethod.Cash;
        }

        public static async Task MarkOrderAsToShippingAfterDelay(DbStorage dbStorageConetxt, int orderId)
        {
            try
            {
                var order = dbStorageConetxt.Orders.Find(orderId);

                if (order == null)
                {
                    consoleLogger.WriteInfo("Order not found.");
                    return;
                }

                consoleLogger.WriteInfo($"Order with ID: {orderId} will be automatically moved to shippng in less than {(double)updateDelay/1000}s.\n" +
                    $"Keep working on your tasks.");


                // TODO: fix this stuff
                var orderData = OrderData.ToDTO(order);

                orderData.Status = OrderStatus.PendingToShipping;
                orderData.MarkToShippingAt = DateTime.Now.AddMilliseconds(updateDelay);

                order = OrderData.ToBO(orderData);

                dbStorageConetxt.SaveChanges();

                await Task.Delay(updateDelay);

                orderData.Status = OrderStatus.InShipping;
                order.Status = orderData.Status;

                dbStorageConetxt.SaveChanges();

                consoleLogger.WriteSuccess($"Order with ID: {orderData.Id} moved to shipping successfully. Follow your previous tasks.\n");
            }
            catch (Exception ex)
            {
                consoleLogger.WriteError(ex.Message);
            }
        }

        #endregion

    }
}
