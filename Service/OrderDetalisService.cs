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
        public static double GetOrderValue()
        {
            while (true)
            {
                Console.Write("Enter order value: ");
                if (double.TryParse(Console.ReadLine(), out double value) && value >= 0)
                {
                    return value;
                }
                Console.WriteLine("Invalid input. Please enter a correct value.");
            }
        }

        public static string GetProductName()
        {
            Console.Write("Enter product name: ");
            return Console.ReadLine();
        }

        public static string GetShippingAddress()
        {
            Console.Write("Enter shipping address: ");
            return Console.ReadLine();
        }

        public static int GetQuantity()
        {
            while (true)
            {
                Console.Write("Enter quantity: ");
                if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                {
                    return quantity;
                }
                Console.WriteLine("Invalid input. Please enter a correct quantity.");
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

        public static string GetCustomerName()
        {
            Console.Write("Enter customer name: ");
            return Console.ReadLine();
        }

        public static PaymentMethod GetPaymentMethod()
        {
            while (true)
            {
                Console.Write("Enter payment method (0 - Cash, 1 - CreditCard): ");
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
