﻿using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using OrderProcessor.Common;
using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.Service
{
    public static class WarehouseService
    {
        public static void MoveToStock(DbStorage dbStorageContext, MessageLogger logger)
        {
            try
            {
                var order = OrderUtility.AskAndFindOrder(dbStorageContext, logger);
                if (order == null) return;

                var orderData = OrderData.ToDTO(order);
                bool isEligible = OrderBusinessLogic.IsOrderEligibleForWarehouseProcessing(orderData);

                if (!isEligible)
                {
                    logger.WriteWarning("Order is not eligible for moving to warehouse.\nReturning order to customer.");
                    orderData.Status = Status.ReturnedToCustomer;
                }
                else
                {
                    orderData.Status = Status.InStock;
                    logger.WriteSuccess("Order moved to stock successfully.");
                }
                order.Status = orderData.Status;

                dbStorageContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }

        public static void MoveToShipping(DbStorage dbStorageContext, MessageLogger logger)
        {
            try
            {
                var orderId = OrderUtility.EnterOrderId(logger);
                Task.Run(async () =>
                    await OrderBusinessLogic.MarkOrderAsShippedAfterDelay(dbStorageContext, orderId)
                );
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }
    }
}
