using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Common
{
    public class UserInputHandler
    {
        public static bool AskUserForConfirmation(string message, ConsoleLogger logger)
        {
            while (true)
            {
                logger.WriteMessageLine($"{message} [Press ENTER to confirm, any other key to cancel]: ");
                var input = Console.ReadKey(true);

                return input.Key == ConsoleKey.Enter;
            }
        }

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

    }
}
