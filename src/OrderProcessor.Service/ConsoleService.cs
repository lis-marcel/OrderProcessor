using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public class ConsoleService
    {
        private readonly DbStorage dbStorageContext;
        private static readonly MessageLogger messageLogger = new();
        private static readonly Dictionary<string, (string Description, Action<DbStorage> Action)> MenuItems =
            new()
            {
                ["1"] = ("Create order", OrderManagmentService.CreateOrder),
                ["2"] = ("Move to stock", OrderManagmentService.MoveToStock),
                ["3"] = ("Move to shipping", OrderManagmentService.MoveToShipping),
                ["4"] = ("Change order status", OrderManagmentService.ChangeOrderStatus),
                ["5"] = ("Edit order details", OrderManagmentService.EditOrderDetails),
                ["6"] = ("Show specific order", OrderManagmentService.ShowSpecificOrder),
                ["7"] = ("Show all orders", OrderManagmentService.ShowAllOrders),
                ["8"] = ("Delete order", OrderManagmentService.DeleteOrder),
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
                messageLogger.WriteError("Failed to connect to database!");
            }
            messageLogger.WriteSuccess("Connected to database successfully.");
        }

        #region Public Methods
        public void Start()
        {
            messageLogger.WriteMessageLine("Welcome to Order Processor");

            while (true)
            {
                var input = SelectedOption();

                if (input != null && MenuItems.TryGetValue(input, out var menuEntry))
                {
                    menuEntry.Action(dbStorageContext);
                }
                else
                {
                    messageLogger.WriteWarning("Invalid input. Please enter a correct value.");
                }

                messageLogger.WriteMessage("\nPress any key to return to the main menu...\n");
                Console.ReadKey();
                Console.Clear();
            }
        }

        #endregion

        #region Private Methods
        private static string SelectedOption()
        {
            while (true)
            {
                ShowMenu();

                messageLogger.WriteMessage("Enter option number: ");
                if (int.TryParse(Console.ReadLine(), out int value) 
                    && value >= 1 && value <= MenuItems.Count)
                {
                    return value.ToString();
                }
                Console.Clear();
                messageLogger.WriteWarning("Invalid input. Please enter a correct value.");
            }
        }

        private static void ShowMenu()
        {
            messageLogger.WriteMessageLine("Select an option:");
            foreach (var entry in MenuItems)
            {
                messageLogger.WriteMessageLine($"{entry.Key}. {entry.Value.Description}");
            }
        }

        private static void ExitApp()
        {
            Console.Clear();
            messageLogger.WriteMessageLine("Exiting the application...");
            Environment.Exit(0);
        }

        #endregion

    }
}
