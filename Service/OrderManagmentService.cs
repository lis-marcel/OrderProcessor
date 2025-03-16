using OrderProcessor.BO;
using OrderProcessor.OrderOptions;
using OrderProcessor.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service
{
    class OrderManagmentService
    {
        #region Public Methods
        public static void CreateOrder(DbStorage dbStorageContext)
        {
            var orderData = CreateOrderDetails();

            int orderId = DbStorageService.GetHighestOrderId(dbStorageContext) + 1;

            orderData.Id = orderId;

            dbStorageContext.Orders.Add(OrderData.OrderDataToOrder(orderData));
            dbStorageContext.SaveChanges();
            Console.WriteLine($"Order created successfully with ID: {orderId}");
        }

        public static int ChangeOrderStatus(DbStorage dbStorageContext)
        {
            int orderId = GetOrderId();

            var order = dbStorageContext.Orders.Find(orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found");
                return -1;
            }
            order.Status = (Status)GetValidOrderStatus();
            dbStorageContext.SaveChanges();
            Console.WriteLine("Order status changed successfully");
            return 0;
        }

        public static void MoveToStock(DbStorage dbStorageContext)
        {
            int orderId = GetOrderId();

            var order = dbStorageContext.Orders.Find(orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found");
                return;
            }
            order.Status = Status.InStock;
            dbStorageContext.SaveChanges();
            Console.WriteLine("Order moved to stock successfully");
        }

        public static void MoveToShipping(DbStorage dbStorageContext)
        {
            int orderId = GetOrderId();

            var order = dbStorageContext.Orders.Find(orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found");
                return;
            }
            order.Status = Status.InShipping;
            dbStorageContext.SaveChanges();
            Console.WriteLine("Order moved to shipping successfully");
        }

        public static void GetOrderById(DbStorage dbStorageContext)
        {
            throw new NotImplementedException();
        }

        public static void ShowAllOrders(DbStorage dbStorageContext)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        private static int GetValidOrderStatus()
        {
            while (true)
            {
                Console.Write("Enter order status (0 - New, 1 - InStock, 2 - InShipping, 3 - ReturnedToCustomer, 4 - Error, 5 - Closed): ");
                if (int.TryParse(Console.ReadLine(), out int status) && Enum.IsDefined(typeof(Status), status))
                {
                    return status;
                }
                Console.WriteLine("Invalid input. Please enter a correct order status.");
            }
        }

        private static int GetOrderId()
        {
            while (true)
            {
                Console.Write("Enter Order ID:");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int orderId))
                {
                    return orderId;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }

        private static OrderData CreateOrderDetails()
        {
            string productName = OrderDetalisService.GetStringValue("Product name");
            double value = OrderDetalisService.GetDoubleValue("order");
            int quantity = OrderDetalisService.GetIntValue("quantity");
            string customerName = OrderDetalisService.GetStringValue("Customer name");
            string shippingAddress = OrderDetalisService.GetStringValue("Shipping address");
            CustomerType customerType = OrderDetalisService.GetCustomerType();
            PaymentMethod paymentMethod = OrderDetalisService.GetPaymentMethod();

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
