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

        public static async Task<(Guid?, Customer?)> LoginCustomer(DbStorage dbStorageContext, CustomerLoginData customerLoginData)
        {
            try
            {
                var customer = dbStorageContext.Customers
                    .Where(c => c.Email == customerLoginData.Email)
                    .FirstOrDefault();

                if (customer == null)
                {
                    return (null, null);
                }

                var authResult = AuthService.AuthCustomer(dbStorageContext, customer);

                if (!authResult.Item1)
                {
                    return (null, null);
                }

                if (customer.LastLoginAt.AddDays(7) > DateTime.Now)
                {
                    
                }

                customer.SessionToken = authResult.Item2;
                customer.LastLoginAt = DateTime.Now;

                await dbStorageContext.SaveChangesAsync();

                return (authResult.Item2, customer);
            }
            catch
            {
                return (null, null);
            }
        }

    }
}
