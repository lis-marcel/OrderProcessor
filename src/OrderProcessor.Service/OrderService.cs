using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using System.Reflection.Metadata.Ecma335;

namespace OrderProcessor.Service
{
    public class OrderService
    {
        #region Public Methods
        public static bool CreateOrder(DbStorage dbStorageContext, OrderCreationData orderCreationData)
        {
            try
            {
                var orderData = OrderUtility.CreateOrderDetails(orderCreationData);

                if (orderData == null)
                {
                    return false;
                }

                dbStorageContext.Orders.Add(OrderData.ToBO(orderData));
                dbStorageContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static OrderData? GetOrder(DbStorage dbStorageContext, int orderId)
        {
            try
            {
                var orderExists = dbStorageContext.Orders.Any(o => o.Id == orderId);
                var order = dbStorageContext.Orders.FirstOrDefault(o => o.Id == orderId);

                return orderExists ? OrderData.ToDTO(order) : null;
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

        public static bool DeleteOrder(DbStorage dbStorageContext, int orderId)
        {
            try
            {
                var orderExists = dbStorageContext.Orders.Any(o => o.Id == orderId);
                var order = dbStorageContext.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    return false;
                }

                var removeResult = dbStorageContext.Orders.Remove(order);
                dbStorageContext.SaveChanges();

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