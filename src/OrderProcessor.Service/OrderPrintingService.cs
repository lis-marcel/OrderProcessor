using OrderProcessor.BO;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Console.Service
{
    public class OrderPrintingService
    {
        private static readonly ConsoleLogger logger = new();

        #region Public Methods
        public static void ShowSpecificOrder(DbStorage dbStorageContext)
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

                var singleOrderCollection = new[] { orderData };

                var tablePrinter = TablePrinter.CreateTable(singleOrderCollection);
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
                    logger.WriteInfo("Orders not found.");
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
        #endregion

    }
}