using OrderProcessor.BO;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public class CustomerService
    {
        public CustomerService() { }

        public static int ValidateCustomerId(DbStorage dbStorageContext, ConsoleLogger consoleLogger)
        {
            int customerId = OrderDetalisService.EnterIntValue("customer ID", consoleLogger);

            var customer = dbStorageContext.Customers.FirstOrDefault(c => c.Id == customerId);

            while (customer == null)
            {
                consoleLogger.WriteWarning($"Customer with ID {customerId} not found.");
                customerId = OrderDetalisService.EnterIntValue("customer ID", consoleLogger);
                customer = dbStorageContext.Customers.FirstOrDefault(c => c.Id == customerId);
            }

            return customerId;
        }
    }
}
