using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public class OrderBusinessLogic
    {
        private static readonly double cashPaymentThreshold = 2500;
        private static readonly MessageLogger messageLogger = new();

        public OrderBusinessLogic() { }

        #region Public Methods
        public static bool IsOrderEligibleForWarehouseProcessing(OrderData orderData)
        {
            bool isEligible = true;

            if (orderData.Value > cashPaymentThreshold && orderData.PaymentMethod == PaymentMethod.OnDelivery)
            {
                isEligible = false;
            }

            return isEligible;
        }

        public static async Task MarkOrderAsShippedAfterDelay(DbStorage dbStorageConetxt, int orderId)
        {
            var order = dbStorageConetxt.Orders.Find(orderId);

            if (order == null)
            {
                messageLogger.WriteError("Order not found.");
                return;
            }

            await Task.Delay(4500);

            var orderData = OrderData.ToDTO(order);

            orderData.Status = Status.InShipping;
            order.Status = orderData.Status;

            dbStorageConetxt.SaveChanges();

            messageLogger.WriteSuccess($"\n Order with ID: {orderData.Id} moved to shipping successfully. Follow the instruction above.\n");
        }

        #endregion

    }
}
