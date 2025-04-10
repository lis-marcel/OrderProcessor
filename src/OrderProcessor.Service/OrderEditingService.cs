﻿using OrderProcessor.BO;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public static class OrderEditingService
    {
        #region Public Methods
        public static void EditOrder(DbStorage dbStorageContext, ConsoleLogger logger)
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
                var orderProperties = typeof(OrderData).GetProperties();

                // Condition for unit tests
                if (Environment.UserInteractive && !Console.IsOutputRedirected)
                {
                    Console.Clear();
                }

                foreach (var property in orderProperties)
                {
                    if (property.Name is "Id" or "CreationTime" or "OrderStatus")
                    {
                        continue;
                    }

                    object oldValue = property.GetValue(orderData);
                    logger.WriteMessageLine($"Current {property.Name}: {oldValue}");

                    if (OrderUtility.AskUserForConfirmation("Do you want to edit this field?", logger))
                    {
                        property.SetValue(orderData, OrderUtility.ParsePropertyValue(property, logger));
                    }
                }

                if (OrderUtility.ShowOrderSummaryAndConfirm(orderData, logger))
                {
                    var updated = OrderData.ToBO(orderData);
                    order.Value = updated.Value;
                    order.ProductName = updated.ProductName;
                    order.ShippingAddress = updated.ShippingAddress;
                    order.Quantity = updated.Quantity;
                    order.CustomerType = updated.CustomerType;
                    order.CustomerName = updated.CustomerName;
                    order.PaymentMethod = updated.PaymentMethod;

                    dbStorageContext.SaveChanges();
                    logger.WriteSuccess("Order details updated successfully.");
                }
                else
                {
                    logger.WriteWarning("Order update cancelled.");
                }
            }
            catch (Exception ex)
            {
                logger.WriteError(ex.Message);
            }
        }
        #endregion

    }
}