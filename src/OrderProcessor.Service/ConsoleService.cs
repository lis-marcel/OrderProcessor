using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public class ConsoleService
    {
        private readonly DbStorage dbStorageContext;
        private static readonly ConsoleLogger consoleLogger = new();
        private static readonly Dictionary<string, (string Description, Action<DbStorage> Action)> MenuItems =
            new()
            {
                ["1"] = ("Create order", OrderManagmentFacade.CreateOrder),
                ["2"] = ("Move to warehouse", OrderManagmentFacade.MoveToWarehouse),
                ["3"] = ("Move to shipping", OrderManagmentFacade.MoveToShipping),
                ["4"] = ("Change order status", OrderManagmentFacade.ChangeOrderStatus),
                ["5"] = ("Edit order details", OrderManagmentFacade.EditOrderDetails),
                ["6"] = ("Show specific order", OrderPrintingService.ShowSpecificOrder),
                ["7"] = ("Show all orders", OrderPrintingService.ShowAllOrders),
                ["8"] = ("Delete order", OrderManagmentFacade.DeleteOrder),
                ["9"] = ("Exit application", _ => ExitApp())
            };

        public ConsoleService() 
        {
            var dbPath = DbStorageService.PrepareDb();

            dbStorageContext = 
                new DbStorage(new DbContextOptionsBuilder<DbStorage>()
                .UseSqlite($"Data Source={dbPath}")
                .Options);

            if (!dbStorageContext.Database.CanConnect())
            {
                consoleLogger.WriteError($"Failed to connect to database located in path {dbPath}");
                consoleLogger.WriteInfo($"Created new database in path {dbPath}");
            }

            consoleLogger.WriteSuccess($"Connected to database {dbPath} successfully.");
        }

        #region Public Methods
        public void Start()
        {
            consoleLogger.WriteMessageLine("Welcome to Order Processor");

            while (true)
            {
                var optionNumber = EnterOptionNumber();

                if (optionNumber != null && MenuItems.TryGetValue(optionNumber, out var menuEntry))
                {
                    menuEntry.Action(dbStorageContext);
                }
                else
                {
                    consoleLogger.WriteWarning("Invalid optionNumber. Please enter a correct value.");
                }

                consoleLogger.WriteMessage("\nPress any key to return to the main menu...\n");
                Console.ReadKey();
                Console.Clear();
            }
        }

        #endregion

        #region Private Methods
        private static string EnterOptionNumber()
        {
            while (true)
            {
                ShowMenu();

                consoleLogger.WriteMessage("Enter option number: ");

                if (int.TryParse(Console.ReadLine(), out int value) 
                    && value >= 1 && value <= MenuItems.Count)
                {
                    return value.ToString();
                }

                Console.Clear();
                consoleLogger.WriteWarning("Invalid optionNumber. Please enter a correct value.");
            }
        }

        private static void ShowMenu()
        {
            consoleLogger.WriteMessageLine("Select an option:");

            foreach (var entry in MenuItems)
            {
                consoleLogger.WriteMessageLine($"{entry.Key}. {entry.Value.Description}");
            }
        }

        private static void ExitApp()
        {
            Console.Clear();
            consoleLogger.WriteMessageLine("Exiting the application...");
            Environment.Exit(0);
        }

        #endregion

    }
}
