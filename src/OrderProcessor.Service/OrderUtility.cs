using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;
using System.Reflection;

namespace OrderProcessor.Service
{
    public static class OrderUtility
    {
        #region Public Methods
        public static Order? AskAndFindOrder(DbStorage dbStorageContext, ConsoleLogger logger)
        {
            Order? res;

            int orderId = EnterOrderId(logger);
            res = dbStorageContext.Orders.Find(orderId);
            
            if (res == null)
            {
                logger.WriteError($"Order {orderId} not found.");
            }
            
            return res;
        }

        public static int EnterOrderId(ConsoleLogger logger)
        {
            while (true)
            {
                logger.WriteMessage("Enter Order ID: ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int orderId))
                {
                    return orderId;
                }

                logger.WriteError("Invalid input. Enter integer type value.");
            }
        }

        public static int GetValidOrderStatus(ConsoleLogger logger)
        {
            while (true)
            {
                logger.WriteMessage("Enter order status (1 - New, 2 - InWarehouse, 3 - PendingToShipping, 4 - InShipping, 5 - ReturnedToCustomer, 6 - Error, 7 - Closed): ");

                if (int.TryParse(Console.ReadLine(), out int status) && Enum.IsDefined(typeof(OrderStatus), status))
                {
                    return status;
                }

                logger.WriteWarning("Invalid input. Please enter a correct order status.");
            }
        }

        public static OrderData? CreateOrderDetails(DbStorage dbStorageContext, ConsoleLogger logger)
        {
            // Clear console if interactive
            if (Environment.UserInteractive && !Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            while (true)
            {
                var orderData = new OrderData
                {
                    ProductName = UserInputHandler.EnterStringValue("product name", logger),
                    Value = UserInputHandler.EnterDoubleValue("order value", logger),
                    Quantity = UserInputHandler.EnterIntValue("quantity", logger),
                    ShippingAddress = UserInputHandler.EnterStringValue("shipping address", logger),
                    CustomerId = CustomerService.ValidateCustomerId(dbStorageContext, UserInputHandler.EnterIntValue("customer ID", logger)),
                    PaymentMethod = OrderDetalisService.EnterPaymentMethod(logger),
                    CreationTime = DateTime.Now,
                    Status = OrderStatus.New
                };

                if (ShowOrderSummaryAndConfirm(orderData, logger))
                {
                    return orderData;
                }

                if (UserInputHandler.AskUserForConfirmation("Do you want to return to the main menu?", logger))
                {
                    return null;
                }

                logger.WriteInfo("Order creation cancelled. Restarting the entry process.");
            }
        }

        public static bool ShowOrderSummaryAndConfirm(OrderData orderData, ConsoleLogger logger)
        {
            if (Environment.UserInteractive && !Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            logger.WriteMessageLine("Order Summary:");
            logger.WriteMessageLine($"Product Name: {orderData.ProductName}");
            logger.WriteMessageLine($"Value: {orderData.Value}");
            logger.WriteMessageLine($"Quantity: {orderData.Quantity}");
            logger.WriteMessageLine($"Shipping Address: {orderData.ShippingAddress}");
            logger.WriteMessageLine($"Customer ID: {orderData.CustomerId}");
            logger.WriteMessageLine($"Payment Method: {orderData.PaymentMethod}");

            return UserInputHandler.AskUserForConfirmation("Do you confirm the order details?", logger);
        }

        public static object? ParsePropertyValue(PropertyInfo property, ConsoleLogger consoleLogger)
        {
            if (property.PropertyType == typeof(int))
            {
                return UserInputHandler.EnterIntValue("new " + property.Name, consoleLogger);
            }
            else if (property.PropertyType == typeof(string))
            {
                return UserInputHandler.EnterStringValue("new " + property.Name, consoleLogger);
            }
            else if (property.PropertyType == typeof(double))
            {
                return UserInputHandler.EnterDoubleValue("new " + property.Name, consoleLogger);
            }
            else if (property.PropertyType == typeof(CustomerType))
            {
                return OrderDetalisService.EnterCustomerType(consoleLogger);
            }
            else if (property.PropertyType == typeof(PaymentMethod))
            {
                return OrderDetalisService.EnterPaymentMethod(consoleLogger);
            }
            return null;
        }

        #endregion


    }
}