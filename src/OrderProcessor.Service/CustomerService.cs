using OrderProcessor.BO;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public class CustomerService
    {
        private static readonly ConsoleLogger consoleLogger = new();

        public CustomerService() { }

        #region Public Methods

        public static int ValidateCustomerId(DbStorage dbStorageContext, int customerId)
        {
            var customer = dbStorageContext.Customers.FirstOrDefault(c => c.Id == customerId);

            while (customer == null)
            {
                consoleLogger.WriteWarning($"Customer with ID {customerId} not found.");

                if (UserInputHandler.AskUserForConfirmation("No custome found! Do you want to add a new customer?", consoleLogger)) 
                {
                    AddCustomer(dbStorageContext);
                }
           
                customerId = UserInputHandler.EnterIntValue("customer ID", consoleLogger);
                customer = dbStorageContext.Customers.FirstOrDefault(c => c.Id == customerId);
            }

            return customerId;
        }

        public static void AddCustomer(DbStorage dbStorageContext)
        {
            try
            {
                var customer = new Customer
                {
                    Name = UserInputHandler.EnterStringValue("customer name", consoleLogger),
                    CustomerType = OrderDetalisService.EnterCustomerType(consoleLogger)
                };

                dbStorageContext.Customers.Add(customer);
                dbStorageContext.SaveChanges();

                int customerId = DbStorageService.GetHighestId<Order>(dbStorageContext) + 1;

                consoleLogger.WriteSuccess($"New customer with ID:{customerId} added successfully.");
            }
            catch (Exception ex)
            {
                consoleLogger.WriteError($"Error adding new customer: {ex.Message}");
                return;
            }
        }

        #endregion

    }
}
