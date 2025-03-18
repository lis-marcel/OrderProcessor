using OrderProcessor.BO;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public class OrderPrintingService
    {
        private static readonly MessageLogger logger = new();

        public static void ShowSpecificOrder(DbStorage dbStorageContext)
        {
            try
            {
                var order = OrderUtility.AskAndFindOrder(dbStorageContext, logger);
                if (order == null) return;

                var orderData = OrderData.ToDTO(order);
                var tablePrinter = TablePrinter.CreateTable(orderData);

                tablePrinter.PrintTable();
                // Print again if needed
                tablePrinter.PrintTable();
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }

        public static void ShowAllOrders(DbStorage dbStorageContext)
        {
            try
            {
                var orders = dbStorageContext.Orders.ToList();
                if (orders.Count == 0)
                {
                    logger.WriteError("No orders found.");
                    return;
                }

                var ordersData = orders.Select(OrderData.ToDTO);
                var tablePrinter = TablePrinter.CreateTable(ordersData);
                tablePrinter.PrintTable();
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }
    }
}