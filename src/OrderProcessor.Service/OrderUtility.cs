using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;
using System.Reflection;

namespace OrderProcessor.Service
{
    public static class OrderUtility
    {
        public static Order AskAndFindOrder(DbStorage dbStorageContext, MessageLogger logger)
        {
            Order? res;

            int orderId = EnterOrderId(logger);
            res = dbStorageContext.Orders.Find(orderId);
            
            if (res == null)
            {
                logger.WriteError( $"Order {orderId} not found.");
            }
            
            return res;
        }

        public static int EnterOrderId(MessageLogger logger)
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

        public static int GetValidOrderStatus(MessageLogger logger)
        {
            while (true)
            {
                logger.WriteMessage("Enter order status (0 - New, 1 - InStock, 2 - InShipping, 3 - ReturnedToCustomer, 4 - Error, 5 - Closed): ");
                if (int.TryParse(Console.ReadLine(), out int status) && Enum.IsDefined(typeof(Status), status))
                {
                    return status;
                }
                logger.WriteError("Invalid input. Please enter a correct order status.");
            }
        }

        public static OrderData CreateOrderDetails(MessageLogger logger)
        {
            // Clear console if interactive
            if (Environment.UserInteractive && !Console.IsOutputRedirected)
            {
                Console.Clear();
            }

            while (true)
            {
                var productName = OrderDetalisService.EnterStringValue("product name");
                var value = OrderDetalisService.EnterDoubleValue("order");
                var quantity = OrderDetalisService.EnterIntValue("quantity");
                var customerName = OrderDetalisService.EnterStringValue("customer name");
                var shippingAddress = OrderDetalisService.EnterStringValue("shipping address");
                var customerType = OrderDetalisService.EnterCustomerType();
                var paymentMethod = OrderDetalisService.EnterPaymentMethod();

                var orderData = new OrderData
                {
                    ProductName = productName,
                    Value = value,
                    Quantity = quantity,
                    CustomerName = customerName,
                    ShippingAddress = shippingAddress,
                    CustomerType = customerType,
                    PaymentMethod = paymentMethod,
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

        public static bool ShowOrderSummaryAndConfirm(OrderData orderData, MessageLogger logger)
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

        public static bool AskUserForConfirmation(string message, MessageLogger logger)
        {
            while (true)
            {
                logger.WriteMessageLine($"{message} [Press ENTER to confirm, any other key to cancel]: ");
                var input = Console.ReadKey(true);
                return input.Key == ConsoleKey.Enter;
            }
        }

        public static object ParsePropertyValue(PropertyInfo property, MessageLogger logger)
        {
            if (property.PropertyType == typeof(int))
            {
                return OrderDetalisService.EnterIntValue(property.Name);
            }
            else if (property.PropertyType == typeof(string))
            {
                return OrderDetalisService.EnterStringValue(property.Name);
            }
            else if (property.PropertyType == typeof(double))
            {
                return OrderDetalisService.EnterDoubleValue(property.Name);
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
    }
}