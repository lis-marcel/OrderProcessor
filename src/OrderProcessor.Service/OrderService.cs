﻿using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public class OrderService
    {
        #region Public Methods
        public static async Task<OperationResult> CreateOrder(DbStorage dbStorageContext, OrderCreationDto orderCreationData)
        {
            try
            {
                var orderData = OrderUtility.CreateOrderDetails(orderCreationData);

                if (orderData == null)
                {
                    return OperationResult.Failed("Order not found.");
                }

                await dbStorageContext.Orders.AddAsync(OrderDto.ToBo(orderData));
                await dbStorageContext.SaveChangesAsync();

                return OperationResult.Succeeded("Order found.", orderData);
            }
            catch
            {
                return OperationResult.Failed("Order not found.");
            }
        }

        public static async Task<OperationResult> GetOrder(DbStorage dbStorageContext, int orderId)
        {
            try
            {
                var order = await dbStorageContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    return OperationResult.Failed("Order not found for given ID.");
                }

                return OperationResult.Succeeded("Order found.", order);
            }
            catch
            {
                return OperationResult.Failed("Error occured during order searching.");
            }
        }

        public static OperationResult GetOrders(DbStorage dbStorageContext)
        {
            try
            {
                var ordersList = dbStorageContext.Orders.Select(o => o).AsNoTracking();

                if (!ordersList.Any())
                {
                    return OperationResult.Failed("No orders found");
                }

                var ordersDtoList = new List<OrderDto>();

                foreach (var order in ordersList)
                {
                    ordersDtoList.Add(OrderDto.ToDto(order));
                }

                return OperationResult.Succeeded("Orders found successfuly.", ordersDtoList);
            }
            catch
            {
                return OperationResult.Failed();
            }
        }

        public static async Task<OperationResult> DeleteOrder(DbStorage dbStorageContext, int orderId)
        {
            try
            {
                var order = dbStorageContext.Orders.Find(orderId);

                if (order == null)
                {
                    return OperationResult.Failed("Order not found for given ID.");
                }

                var removeResult = dbStorageContext.Orders.Remove(order);
                await dbStorageContext.SaveChangesAsync();

                return OperationResult.Succeeded("Order removed successfuly.");
            }
            catch
            {
                return OperationResult.Failed();
            }
        }
        #endregion

    }
}