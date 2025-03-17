using OrderProcessor.BO;
using OrderProcessor.OrderOptions;
using OrderProcessor.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service
{
    public class OrderBusinessLogic
    {
        private static readonly double cashPaymentThreshold = 2500;

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

        public static async Task MarkOrderAsShippedAfterDelay(DbStorage dbStorageConetxt, Order order, OrderData orderData)
        {
            await Task.Delay(4500);

            orderData.Status = Status.InShipping;
            order.Status = orderData.Status;

            dbStorageConetxt.SaveChanges();

            Console.WriteLine($"\n[INFO] Order with ID: {orderData.Id} moved to shipping successfully. Follow the instruction above.\n");
        }

        #endregion

    }
}
