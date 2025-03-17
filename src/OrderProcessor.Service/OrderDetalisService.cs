using OrderProcessor.OrderOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service
{
    class OrderDetalisService
    {
        #region Public Methods
        public static double GetDoubleValue(string fieldName)
        {
            while (true)
            {
                Console.Write($"Enter {fieldName} value: ");
                if (double.TryParse(Console.ReadLine(), out double value) && value >= 0)
                {
                    return value;
                }
                Console.WriteLine($"Invalid input. Please enter a correct {fieldName} value.");
            }
        }

        public static string GetStringValue(string fieldName)
        {
            while (true)
            {
                Console.Write($"Enter {fieldName}: ");
                string? productName = Console.ReadLine();
                if (!string.IsNullOrEmpty(productName))
                {
                    return productName;
                }
                Console.Clear();
                Console.WriteLine($"Invalid input. {fieldName} can not be empty.");
            }
        }

        public static int GetIntValue(string fieldName)
        {
            while (true)
            {
                Console.Write($"Enter {fieldName}: ");
                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                {
                    return quantity;
                }
                Console.WriteLine($"Invalid input. Please enter a correct {fieldName}.");
            }
        }

        public static CustomerType GetCustomerType()
        {
            while (true)
            {
                Console.Write("Enter customer type (0 - Individual, 1 - Company): ");
                if (int.TryParse(Console.ReadLine(), out int customerType) && Enum.IsDefined(typeof(CustomerType), customerType))
                {
                    return (CustomerType)customerType;
                }
                Console.WriteLine("Invalid input. Please enter a correct customer type.");
            }
        }

        public static PaymentMethod GetPaymentMethod()
        {
            while (true)
            {
                Console.Write("Enter payment method (0 - Cash, 1 - Credit card, 2 - On delivery): ");
                if (int.TryParse(Console.ReadLine(), out int paymentMethod) && Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                {
                    return (PaymentMethod)paymentMethod;
                }
                Console.WriteLine("Invalid input. Please enter a correct payment method.");
            }
        }
        #endregion

    }
}
