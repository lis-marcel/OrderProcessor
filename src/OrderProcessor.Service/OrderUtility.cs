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
        public static Order AskAndFindOrder(DbStorage dbStorageContext, ConsoleLogger logger)
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
                logger.WriteMessage("Enter order status (1 - New, 2 - InStock, 3 - InShipping, 4 - ReturnedToCustomer, 5 - Error, 6 - Closed): ");

                if (int.TryParse(Console.ReadLine(), out int status) && Enum.IsDefined(typeof(Status), status))
                {
                    return status;
                }

                logger.WriteWarning("Invalid input. Please enter a correct order status.");
            }
        }

        public static OrderData CreateOrderDetails(ConsoleLogger logger)
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
                    ProductName = OrderDetalisService.EnterStringValue("product name"),
                    Value = OrderDetalisService.EnterDoubleValue("order value"),
                    Quantity = OrderDetalisService.EnterIntValue("quantity"),
                    CustomerName = OrderDetalisService.EnterStringValue("customer name"),
                    ShippingAddress = OrderDetalisService.EnterStringValue("shipping address"),
                    CustomerType = OrderDetalisService.EnterCustomerType(),
                    PaymentMethod = OrderDetalisService.EnterPaymentMethod(),
                    CreationTime = DateTime.Now,
                    Status = Status.New
                };

                if (ShowOrderSummaryAndConfirm(orderData, logger))
                {
                    return orderData;
                }

                if (AskUserForConfirmation("Do you want to return to the main menu?", logger))
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
            logger.WriteMessageLine($"Customer Name: {orderData.CustomerName}");
            logger.WriteMessageLine($"Shipping Address: {orderData.ShippingAddress}");
            logger.WriteMessageLine($"Customer Type: {orderData.CustomerType}");
            logger.WriteMessageLine($"Payment Method: {orderData.PaymentMethod}");

            return AskUserForConfirmation("Do you confirm the order details?", logger);
        }

        public static bool AskUserForConfirmation(string message, ConsoleLogger logger)
        {
            while (true)
            {
                logger.WriteMessageLine($"{message} [Press ENTER to confirm, any other key to cancel]: ");
                var input = Console.ReadKey(true);

                return input.Key == ConsoleKey.Enter;
            }
        }

        public static object ParsePropertyValue(PropertyInfo property, ConsoleLogger logger)
        {
            if (property.PropertyType == typeof(int))
            {
                return OrderDetalisService.EnterIntValue("new " + property.Name);
            }
            else if (property.PropertyType == typeof(string))
            {
                return OrderDetalisService.EnterStringValue("new " + property.Name);
            }
            else if (property.PropertyType == typeof(double))
            {
                return OrderDetalisService.EnterDoubleValue("new " + property.Name);
            }
            else if (property.PropertyType == typeof(CustomerType))
            {
                return OrderDetalisService.EnterCustomerType();
            }
            else if (property.PropertyType == typeof(PaymentMethod))
            {
                return OrderDetalisService.EnterPaymentMethod();
            }
            return null;
        }
        #endregion

    }
}