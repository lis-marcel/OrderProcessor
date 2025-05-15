using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.BO.Entities;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public class CustomerService
    {
        public CustomerService() { }

        public static async Task<bool> RegisterCustomer(DbStorage dbStorageContext, CustomerRegistrationData customerRegistrationData)
        {
            try
            {
                var customerData = CustomerUtility.CreateCustomerDetails(customerRegistrationData);

                if (customerData == null)
                {
                    return false;
                }

                await dbStorageContext.Customers.AddAsync(CustomerData.ToBO(customerData));
                await dbStorageContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<(string?, Customer?)> LoginCustomer(
            DbStorage dbStorageContext,
            CustomerLoginData customerLoginData,
            TokenService tokenService)
        {
            try
            {
                var (isAuthenticated, customer) = AuthService.AuthenticateCustomer(
                    dbStorageContext,
                    customerLoginData.Email!,
                    customerLoginData.Password!);

                if (!isAuthenticated || customer == null)
                {
                    return (null, null);
                }

                // Generate JWT token
                var token = tokenService.GenerateJwtToken(customer);

                customer.LastLoginAt = DateTime.Now;
                await dbStorageContext.SaveChangesAsync();

                return (token, customer);
            }
            catch
            {
                return (null, null);
            }
        }

        public static List<Order> GetCustomerOrders(DbStorage dbStorageContext, string email)
        {
            try
            {
                var customer = dbStorageContext.Customers
                    .Where(c => c.Email == email)
                    .AsNoTracking()
                    .FirstOrDefault();

                if (customer == null)
                {
                    return new List<Order>();
                }

                var orders = dbStorageContext.Orders
                    .Where(o => o.CustomerId == customer.Id)
                    .ToList();

                return orders;
            }
            catch
            {
                return new List<Order>();
            }
        }


    }
}
