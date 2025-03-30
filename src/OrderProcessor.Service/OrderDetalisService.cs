using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public class OrderDetalisService
    {
        #region Public Methods
        public static double EnterDoubleValue(string fieldName, ConsoleLogger consoleLogger)
        {
            while (true)
            {
                consoleLogger.WriteMessage($"Enter {fieldName}: ");

                if (double.TryParse(Console.ReadLine(), out double value) && value >= 0)
                {
                    return value;
                }

                consoleLogger.WriteWarning($"Invalid input. Please enter a correct {fieldName} value.");
            }
        }

        public static string EnterStringValue(string fieldName, ConsoleLogger consoleLogger)
        {
            while (true)
            {
                consoleLogger.WriteMessage($"Enter {fieldName}: ");
                string? productName = Console.ReadLine();

                if (!string.IsNullOrEmpty(productName))
                {
                    return productName;
                }

                consoleLogger.WriteWarning($"Invalid input. {fieldName} can not be empty.");
            }
        }

        public static int EnterIntValue(string fieldName, ConsoleLogger consoleLogger)
        {
            while (true)
            {
                consoleLogger.WriteMessage($"Enter {fieldName}: ");

                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                {
                    return quantity;
                }

                consoleLogger.WriteWarning($"Invalid input. Please enter a correct {fieldName}.");
            }
        }

        public static CustomerType EnterCustomerType(ConsoleLogger consoleLogger)
        {
            while (true)
            {
                consoleLogger.WriteMessage("Enter customer type (1 - Individual, 2 - Company): ");

                if (int.TryParse(Console.ReadLine(), out int customerType) && Enum.IsDefined(typeof(CustomerType), customerType))
                {
                    return (CustomerType)customerType;
                }

                consoleLogger.WriteWarning("Invalid input. Please enter a correct customer type.");
            }
        }

        public static PaymentMethod EnterPaymentMethod(ConsoleLogger consoleLogger)
        {
            while (true)
            {
                consoleLogger.WriteMessage("Enter payment method (1 - Cash on delivery, 2 - Credit card): ");

                if (int.TryParse(Console.ReadLine(), out int paymentMethod) && Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                {
                    return (PaymentMethod)paymentMethod;
                }

                consoleLogger.WriteWarning("Invalid input. Please enter a correct payment method.");
            }
        }

        #endregion

    }
}
