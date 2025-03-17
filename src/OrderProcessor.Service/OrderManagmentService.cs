using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public class OrderManagmentService
    {
        private static readonly MessageLogger messageLogger = new();

        #region Public Methods
        public static void CreateOrder(DbStorage dbStorageContext)
        {
            var orderData = CreateOrderDetails();

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
            var productName = OrderDetalisService.GetStringValue("Product name");
            var value = OrderDetalisService.GetDoubleValue("order");
            var quantity = OrderDetalisService.GetIntValue("quantity");
            var customerName = OrderDetalisService.GetStringValue("Customer name");
            var shippingAddress = OrderDetalisService.GetStringValue("Shipping address");
            var customerType = OrderDetalisService.GetCustomerType();
            var paymentMethod = OrderDetalisService.GetPaymentMethod();

            OrderData order = new()
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

            return order;
        }

        #endregion

    }
}
