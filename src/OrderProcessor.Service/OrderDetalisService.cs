using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;

namespace OrderProcessor.Console.Service
{
    public class OrderDetalisService
    {
        #region Public Methods
        public static PaymentMethod EnterPaymentMethod(ConsoleLogger consoleLogger)
        {
            while (true)
            {
                consoleLogger.WriteMessage("Enter payment method (1 - Cash on delivery, 2 - Credit card): ");

                if (int.TryParse(System.Console.ReadLine(), out int paymentMethod) && Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                {
                    return (PaymentMethod)paymentMethod;
                }

                consoleLogger.WriteWarning("Invalid input. Please enter a correct payment method.");
            }
        }

        public static CustomerType EnterCustomerType(ConsoleLogger consoleLogger)
        {
            while (true)
            {
                consoleLogger.WriteMessage("Enter customer type (1 - Individual, 2 - Company): ");

                if (int.TryParse(System.Console.ReadLine(), out int customerType) && Enum.IsDefined(typeof(CustomerType), customerType))
                {
                    return (CustomerType)customerType;
                }

                consoleLogger.WriteWarning("Invalid input. Please enter a correct customer type.");
            }
        }

        #endregion

    }
}
