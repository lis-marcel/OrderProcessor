using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;
using System.Reflection;

namespace OrderProcessor.Service
{
    public class OrderManagmentFacade
    {
        private static readonly ConsoleLogger messageLogger = new();

        #region Public Methods
        public static void CreateOrder(DbStorage dbStorageContext)
        {
            OrderCreationService.CreateOrder(dbStorageContext, messageLogger);
        }

        public static void ChangeOrderStatus(DbStorage dbStorageContext)
        {
            OrderStatusService.ChangeStatus(dbStorageContext, messageLogger);
        }

        public static void MoveToStock(DbStorage dbStorageContext)
        {
            WarehouseService.MoveToStock(dbStorageContext, messageLogger);
        }

        public static void MoveToShipping(DbStorage dbStorageContext)
        {
            WarehouseService.MoveToShipping(dbStorageContext, messageLogger);
        }

        public static void EditOrderDetails(DbStorage dbStorageContext)
        {
            OrderEditingService.EditOrder(dbStorageContext, messageLogger);
        }

        public static void DeleteOrder(DbStorage dbStorageContext)
        {
            try
            {
                var order = OrderUtility.AskAndFindOrder(dbStorageContext, messageLogger);

                if (order == null)
                {
                    messageLogger.WriteInfo("Order not found.");
                    return;
                }

                dbStorageContext.Orders.Remove(order);
                dbStorageContext.SaveChanges();
                messageLogger.WriteSuccess("Order deleted successfully.");
            }
            catch (Exception ex)
            {
                messageLogger.WriteError(ex.Message);
            }
        }
        #endregion

    }
}
