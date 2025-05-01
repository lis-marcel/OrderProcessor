using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using System.Reflection.Metadata.Ecma335;

namespace OrderProcessor.Service
{
    public class OrderService
    {
        #region Public Methods
        public static async Task<bool> CreateOrder(DbStorage dbStorageContext, OrderCreationData orderCreationData)
        {
            try
            {
                var orderData = await OrderUtility.CreateOrderDetails(orderCreationData);

                if (orderData == null)
                {
                    return false;
                }

                await dbStorageContext.Orders.AddAsync(OrderData.ToBO(orderData));
                await dbStorageContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<OrderData?> GetOrder(DbStorage dbStorageContext, int orderId)
        {
            try
            {
                var orderExists = await dbStorageContext.Orders.AnyAsync(o => o.Id == orderId);
                var order = await dbStorageContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                return orderExists ? OrderData.ToDTO(order!) : null;
            }
            catch
            {
                return null;
            }
        }

        public static List<OrderData>? GetOrders(DbStorage dbStorageContext)
        {
            try
            {
                var ordersList = dbStorageContext.Orders.Select(o => o);

                var ordersDataList = new List<OrderData>();

                foreach (var order in ordersList)
                {
                    ordersDataList.Add(OrderData.ToDTO(order));
                }

                return ordersDataList;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> DeleteOrder(DbStorage dbStorageContext, int orderId)
        {
            try
            {
                var orderExists = await dbStorageContext.Orders.AnyAsync(o => o.Id == orderId);
                var order = await dbStorageContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    return false;
                }

                var removeResult = dbStorageContext.Orders.Remove(order);

                if (removeResult == null)
                {
                    return false;
                }

                await dbStorageContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}