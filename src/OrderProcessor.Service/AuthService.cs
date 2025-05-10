using OrderProcessor.BO;
using OrderProcessor.BO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service
{
    public class AuthService
    {
        public AuthService() { }

        // This method is just a simplified example of how to authenticate a customer. 
        // Will be replaced with a more secure implementation in the future.
        public static (bool, Guid) AuthCustomer(DbStorage dbStorage, Customer customer)
        {
            try
            {
                var existingCustomer = dbStorage.Customers
                    .Where(c => c.Email == customer.Email && c.Password == customer.Password)
                    .FirstOrDefault();

                if (existingCustomer == null)
                {
                    return (false, Guid.Empty);
                }

                return (true, Guid.NewGuid());
            }
            catch
            {
                return (false, Guid.Empty);
            }
        }

        public static (bool, Guid) AuthCustomer(DbStorage dbStorage, Guid guid)
        {
            try
            {
                var customerSession = dbStorage.Customers
                    .Where(c => c.SessionToken == guid)
                    .FirstOrDefault();

                if (customerSession == null)
                {
                    return (false, Guid.Empty);
                }

                customerSession.LastLoginAt = DateTime.Now;

                dbStorage.SaveChanges();

                return (true, customerSession.SessionToken);
            }
            catch
            {
                return (false, Guid.Empty);
            }
        }
    }
}
