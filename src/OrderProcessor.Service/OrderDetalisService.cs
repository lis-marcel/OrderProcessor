using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public class OrderDetalisService
    {
        private static readonly MessageLogger messageLogger = new();

        #region Public Methods
        public static double EnterDoubleValue(string fieldName)
        {
            while (true)
            {
                messageLogger.WriteMessage($"Enter {fieldName} value: ");
                if (double.TryParse(Console.ReadLine(), out double value) && value >= 0)
                {
                    return value;
                }
                messageLogger.WriteWarning($"Invalid input. Please enter a correct {fieldName} value.");
            }
        }

        public static string EnterStringValue(string fieldName)
        {
            while (true)
            {
                messageLogger.WriteMessage($"Enter {fieldName}: ");
                string? productName = Console.ReadLine();
                if (!string.IsNullOrEmpty(productName))
                {
                    return productName;
                }
                messageLogger.WriteWarning($"Invalid input. {fieldName} can not be empty.");
            }
        }

        public static int EnterIntValue(string fieldName)
        {
            while (true)
            {
                messageLogger.WriteMessage($"Enter {fieldName}: ");
                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                {
                    return quantity;
                }
                messageLogger.WriteWarning($"Invalid input. Please enter a correct {fieldName}.");
            }
        }

        public static CustomerType EnterCustomerType()
        {
            while (true)
            {
                messageLogger.WriteMessage("Enter customer type (0 - Individual, 1 - Company): ");
                if (int.TryParse(Console.ReadLine(), out int customerType) && Enum.IsDefined(typeof(CustomerType), customerType))
                {
                    return (CustomerType)customerType;
                }
                messageLogger.WriteWarning("Invalid input. Please enter a correct customer type.");
            }
        }

        public static PaymentMethod EnterPaymentMethod()
        {
            while (true)
            {
                messageLogger.WriteMessage("Enter payment method (0 - Cash on delivery, 1 - Credit card): ");
                if (int.TryParse(Console.ReadLine(), out int paymentMethod) && Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                {
                    return (PaymentMethod)paymentMethod;
                }
                messageLogger.WriteWarning("Invalid input. Please enter a correct payment method.");
            }
        }

        #endregion

    }
}
