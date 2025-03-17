﻿using OrderProcessor.BO;
using OrderProcessor.OrderOptions;
using OrderProcessor.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service
{
    public class OrderManagmentService
    {
        #region Public Methods
        public static void CreateOrder(DbStorage dbStorageContext)
        {
            var orderData = CreateOrderDetails();

            int orderId = DbStorageService.GetHighestOrderId(dbStorageContext) + 1;

            orderData.Id = orderId;

            dbStorageContext.Orders.Add(OrderData.ToOrder(orderData));
            dbStorageContext.SaveChanges();
            Console.WriteLine($"Order created successfully with ID: {orderId}");
        }

        public static int ChangeOrderStatus(DbStorage dbStorageContext)
        {
            int orderId = GetOrderId();

            var order = dbStorageContext.Orders.Find(orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return -1;
            }

            var orderData = OrderData.ToOrderData(order);
            orderData.Status = (Status)GetValidOrderStatus();

            order.Status = orderData.Status;

            dbStorageContext.SaveChanges();
            Console.WriteLine("Order status changed successfully.");
            return 0;
        }

        public static void MoveToStock(DbStorage dbStorageContext)
        {
            int orderId = GetOrderId();

            var order = dbStorageContext.Orders.Find(orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            var orderData = OrderData.ToOrderData(order);
            bool isEligible = OrderBusinessLogic.IsOrderEligibleForWarehouseProcessing(orderData);
            if (!isEligible)
            {
                Console.WriteLine("Order is not eligible for moving to warehouse.\n" +
                    "Returning order to customer.");

                orderData.Status = Status.ReturnedToCustomer;
            }
            else 
            { 
                orderData.Status = Status.InStock;
                Console.WriteLine("Order moved to stock successfully.");
            }

            order.Status = orderData.Status;

            dbStorageContext.SaveChanges();
        }

        public static void MoveToShipping(DbStorage dbStorageContext)
        {
            int orderId = GetOrderId();

            var order = dbStorageContext.Orders.Find(orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            var orderData = OrderData.ToOrderData(order);

            Task.Run(async () => await OrderBusinessLogic.MarkOrderAsShippedAfterDelay(dbStorageContext, order, orderData));
        }

        public static void ShowSpecificOrder(DbStorage dbStorageContext)
        {
            int orderId = GetOrderId();

            var order = dbStorageContext.Orders.Find(orderId);

            if (order is null)
            {
                Console.WriteLine("No order found for given ID.");
                return;
            }

            var orderData = OrderData.ToOrderData(order);

            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine("| ID  | Value   | Product Name         | Address              | Qty | Status    | Payment  |");
            Console.WriteLine("+------------------------------------------------------------------------------------------+");

            Console.WriteLine(
                $"| {orderData.Id,-4}|" +
                $" {orderData.Value,-8}|" +
                $" {orderData.ProductName,-21}|" +
                $" {orderData.ShippingAddress,-21}|" +
                $" {orderData.Quantity,-4}|" +
                $" {orderData.Status,-10}|" +
                $" {orderData.PaymentMethod,-8} |"
            );

            Console.WriteLine("--------------------------------------------------------------------------------------------");
        }

        public static void ShowAllOrders(DbStorage dbStorageContext)
        {
            var orders = dbStorageContext.Orders.ToList();

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine("| ID  | Value   | Product Name         | Address              | Qty | Status    | Payment  |");
            Console.WriteLine("+------------------------------------------------------------------------------------------+");

            foreach (var order in orders)
            {
                Console.WriteLine(
                    $"| {order.Id,-4}|" +
                    $" {order.Value,-8}|" +
                    $" {order.ProductName,-21}|" +
                    $" {order.ShippingAddress,-21}|" +
                    $" {order.Quantity,-4}|" +
                    $" {order.Status,-10}|" +
                    $" {order.PaymentMethod,-8} |"
                );
            }

            Console.WriteLine("--------------------------------------------------------------------------------------------");
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
                Console.Write("Enter Order ID: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int orderId))
                {
                    return orderId;
                }
                else
                {
                    Console.WriteLine("Invalid input. Enter intiger type value.");
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
