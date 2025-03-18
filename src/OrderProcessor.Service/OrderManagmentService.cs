using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;
using System.Reflection;

namespace OrderProcessor.Service
{
    public class OrderManagmentService
    {
        private static readonly MessageLogger messageLogger = new();

        #region Public Methods
        public static void CreateOrder(DbStorage dbStorageContext)
        {
            var orderData = CreateOrderDetails();

            if (orderData == null)
            {
                messageLogger.WriteWarning("Order creation cancelled.");
                return;
            }

            int orderId = DbStorageService.GetHighestOrderId(dbStorageContext) + 1;

            orderData.Id = orderId;

            dbStorageContext.Orders.Add(OrderData.ToBO(orderData));
            dbStorageContext.SaveChanges();
            messageLogger.WriteSuccess($"Order created successfully with ID: {orderId}");
        }

        public static void ChangeOrderStatus(DbStorage dbStorageContext)
        {
            var order = GetOrder(dbStorageContext);
            if (order == null)
            {
                return;
            }

            var orderData = OrderData.ToDTO(order);

            orderData.Status = (Status)GetValidOrderStatus();

            order.Status = orderData.Status;

            dbStorageContext.SaveChanges();
            messageLogger.WriteSuccess("Order status changed successfully.");
        }

        public static void MoveToStock(DbStorage dbStorageContext)
        {
            var order = GetOrder(dbStorageContext);
            if (order == null)
            {
                return;
            }

            var orderData = OrderData.ToDTO(order);

            bool isEligible = OrderBusinessLogic.IsOrderEligibleForWarehouseProcessing(orderData);
            if (!isEligible)
            {
                messageLogger.WriteWarning("Order is not eligible for moving to warehouse.\n" +
                    "Returning order to customer.");

                orderData.Status = Status.ReturnedToCustomer;
            }
            else 
            { 
                orderData.Status = Status.InStock;
                messageLogger.WriteSuccess("Order moved to stock successfully.");
            }

            order.Status = orderData.Status;

            dbStorageContext.SaveChanges();
        }

        public static void MoveToShipping(DbStorage dbStorageContext)
        {
            var orderId = GetOrderId();

            Task.Run(async () => await OrderBusinessLogic.MarkOrderAsShippedAfterDelay(dbStorageContext, orderId));
        }

        public static void EditOrderDetails(DbStorage dbStorageContext)
        {
            var order = GetOrder(dbStorageContext);
            if (order == null)
            {
                return;
            }

            var orderData = OrderData.ToDTO(order);
            var orderProperties = typeof(OrderData).GetProperties();

            // Clear the console before displaying the order details for editing
            Console.Clear();

            foreach (var property in orderProperties)
            {
                if (property.Name == "Id" || property.Name == "CreationTime" || property.Name == "Status")
                {
                    continue;
                }

                object oldValue = property.GetValue(orderData);

                messageLogger.WriteMessageLine($"Current {property.Name}: {oldValue}");

                if (AskUserForConfirmation("Do you want to edit this field?"))
                {
                    property.SetValue(orderData, ParsePropertyValue(property));
                }
            }

            if (ShowOrderSummaryAndConfirm(orderData))
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
                messageLogger.WriteSuccess("Order details updated successfully.");
            }

            messageLogger.WriteWarning("Order update cancelled.");
        }

        public static void ShowSpecificOrder(DbStorage dbStorageContext)
        {
            var order = GetOrder(dbStorageContext);
            if (order == null)
            {
                return;
            }

            var orderData = OrderData.ToDTO(order);

            messageLogger.WriteMessageLine("--------------------------------------------------------------------------------------------");
            messageLogger.WriteMessageLine("| ID  | Value   | Product Name         | Address              | Qty | Status    | Payment  |");
            messageLogger.WriteMessageLine("+------------------------------------------------------------------------------------------+");

            messageLogger.WriteMessageLine(
                $"| {orderData.Id,-4}|" +
                $" {orderData.Value,-8}|" +
                $" {orderData.ProductName,-21}|" +
                $" {orderData.ShippingAddress,-21}|" +
                $" {orderData.Quantity,-4}|" +
                $" {orderData.Status,-10}|" +
                $" {orderData.PaymentMethod,-8} |"
            );

            messageLogger.WriteMessageLine("--------------------------------------------------------------------------------------------");
        }

        public static void ShowAllOrders(DbStorage dbStorageContext)
        {
            var orders = dbStorageContext.Orders.ToList();

            if (orders.Count == 0)
            {
                messageLogger.WriteError("No orders found.");
                return;
            }

            messageLogger.WriteMessageLine("--------------------------------------------------------------------------------------------");
            messageLogger.WriteMessageLine("| ID  | Value   | Product Name         | Address              | Qty | Status    | Payment  |");
            messageLogger.WriteMessageLine("+------------------------------------------------------------------------------------------+");

            foreach (var order in orders)
            {
                messageLogger.WriteMessageLine(
                    $"| {order.Id,-4}|" +
                    $" {order.Value,-8}|" +
                    $" {order.ProductName,-21}|" +
                    $" {order.ShippingAddress,-21}|" +
                    $" {order.Quantity,-4}|" +
                    $" {order.Status,-10}|" +
                    $" {order.PaymentMethod,-8} |"
                );
            }

            messageLogger.WriteMessageLine("--------------------------------------------------------------------------------------------");
        }

        public static void DeleteOrder(DbStorage dbStorageContext)
        {
            var order = GetOrder(dbStorageContext);
            if (order == null)
            {
                return;
            }

            dbStorageContext.Orders.Remove(order);
            dbStorageContext.SaveChanges();
            messageLogger.WriteSuccess("Order deleted successfully.");
        }

        #endregion

        #region Private Methods
        private static Order GetOrder(DbStorage dbStorageContext)
        {
            int orderId = GetOrderId();

            var order = dbStorageContext.Orders.Find(orderId);
            if (order == null)
            {
                messageLogger.WriteError("Order not found.");
                return null;
            }

            return order;
        }

        private static int GetValidOrderStatus()
        {
            while (true)
            {
                messageLogger.WriteMessage("Enter order status (0 - New, 1 - InStock, 2 - InShipping, 3 - ReturnedToCustomer, 4 - Error, 5 - Closed): ");
                if (int.TryParse(Console.ReadLine(), out int status) && Enum.IsDefined(typeof(Status), status))
                {
                    return status;
                }
                messageLogger.WriteError("Invalid input. Please enter a correct order status.");
            }
        }

        private static int GetOrderId()
        {
            while (true)
            {
                messageLogger.WriteMessage("Enter Order ID: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int orderId))
                {
                    return orderId;
                }
                else
                {
                    messageLogger.WriteError("Invalid input. Enter intiger type value.");
                }
            }
        }

        private static OrderData CreateOrderDetails()
        {
            Console.Clear();

            while (true)
            {
                var productName = OrderDetalisService.GetStringValue("product name");
                var value = OrderDetalisService.GetDoubleValue("order");
                var quantity = OrderDetalisService.GetIntValue("quantity");
                var customerName = OrderDetalisService.GetStringValue("customer name");
                var shippingAddress = OrderDetalisService.GetStringValue("shipping address");
                var customerType = OrderDetalisService.GetCustomerType();
                var paymentMethod = OrderDetalisService.GetPaymentMethod();


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
                    Status = Status.New,
                };

                if (ShowOrderSummaryAndConfirm(orderData))
                {
                    return orderData;
                }

                if (AskUserForConfirmation("Do you want to return to the main menu?"))
                {
                    return null;
                }

                messageLogger.WriteInfo("Order creation cancelled. Restarting the entry process.");
            }
        }

        private static bool ShowOrderSummaryAndConfirm(OrderData orderData)
        {
            Console.Clear();
            messageLogger.WriteMessageLine("Order Summary:");
            messageLogger.WriteMessageLine($"Product Name: {orderData.ProductName}");
            messageLogger.WriteMessageLine($"Value: {orderData.Value}");
            messageLogger.WriteMessageLine($"Quantity: {orderData.Quantity}");
            messageLogger.WriteMessageLine($"Customer Name: {orderData.CustomerName}");
            messageLogger.WriteMessageLine($"Shipping Address: {orderData.ShippingAddress}");
            messageLogger.WriteMessageLine($"Customer Type: {orderData.CustomerType}");
            messageLogger.WriteMessageLine($"Payment Method: {orderData.PaymentMethod}");

            return AskUserForConfirmation("Do you confirm the order details?");
        }

        private static bool AskUserForConfirmation(string message)
        {
            while (true)
            {
                messageLogger.WriteMessageLine(
                    $"{message} [Press ENTER to confirm, any other key to cancel]: "
                );
                var input = Console.ReadKey(intercept: true);

                // Enter = true, anything else = false
                return input.Key == ConsoleKey.Enter;
            }
        }

        private static object ParsePropertyValue(PropertyInfo property)
        {
            if (property.PropertyType == typeof(int))
            {
                int newValue = OrderDetalisService.GetIntValue(property.Name);
                return newValue;
            }
            else if (property.PropertyType == typeof(string))
            {
                string newValue = OrderDetalisService.GetStringValue(property.Name);
                return newValue;
            }
            else if (property.PropertyType == typeof(double))
            {
                double newValue = OrderDetalisService.GetDoubleValue(property.Name);
                return newValue;
            }
            else if (property.PropertyType == typeof(CustomerType))
            {
                CustomerType customerType = OrderDetalisService.GetCustomerType();
                return customerType;
            }
            else if (property.PropertyType == typeof(PaymentMethod))
            {
                PaymentMethod paymentMethod = OrderDetalisService.GetPaymentMethod();
                return paymentMethod;
            }
            return null;
        }

        #endregion

    }
}
