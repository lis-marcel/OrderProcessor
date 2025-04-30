using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public class OrderBusinessLogic
    {
        private static readonly double cashPaymentThreshold = 2500;
        private static readonly int updateDelay = 4500;

        public OrderBusinessLogic() { }

        #region Public Methods
        public static bool IsOrderEligibleForWarehouseProcessing(OrderData orderData)
        {
            return orderData.PaymentMethod == PaymentMethod.Cash && orderData.Value > cashPaymentThreshold ? false : true;
        }

        public static async Task MarkOrderAsToShippingAfterDelay(DbStorage dbStorageConetxt, int orderId)
        {
            try
            {
                var order = dbStorageConetxt.Orders.Find(orderId);

                if (order == null)
                {
                    return;
                }

                var orderData = OrderData.ToDTO(order);

                // Set the order to pending to shipping
                orderData.Status = OrderStatus.PendingToShipping;
                orderData.MarkToShippingAt = DateTime.Now.AddMilliseconds(updateDelay);

                order.Status = orderData.Status;
                order.MarkToShippingAt = orderData.MarkToShippingAt;

                dbStorageConetxt.SaveChanges();

                // Imitate a delay
                await Task.Delay(updateDelay);

                orderData.Status = OrderStatus.InShipping;
                order.Status = orderData.Status;

                dbStorageConetxt.SaveChanges();

            }
            catch (Exception)
            {
            }
        }

        #endregion

    }
}
