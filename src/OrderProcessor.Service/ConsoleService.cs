using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using Microsoft.EntityFrameworkCore.Sqlite;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;

namespace OrderProcessor.Service
{
    public class ConsoleService
    {
        private readonly DbStorage dbStorageContext;
        private static readonly MessageLogger messageLogger = new();

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

                switch (input)
                {
                    case "1":
                        OrderManagmentService.CreateOrder(dbStorageContext);
                        break;
                    case "2":
                        OrderManagmentService.MoveToStock(dbStorageContext);
                        break;
                    case "3":
                        OrderManagmentService.MoveToShipping(dbStorageContext);
                        break;
                    case "4":
                        OrderManagmentService.ChangeOrderStatus(dbStorageContext);
                        break;
                    case "5":
                        OrderManagmentService.ShowSpecificOrder(dbStorageContext);
                        break;
                    case "6":
                        OrderManagmentService.ShowAllOrders(dbStorageContext);
                        break;
                    case "7":
                        ExitApp();
                        return;
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
                if (int.TryParse(Console.ReadLine(), out int value) && value >= 0 && value <= Enum.GetNames(typeof(Operation)).Length)
                {
                    return value.ToString();
                }
                Console.Clear();
                messageLogger.WriteWarning("Invalid input. Please enter a correct value.");
            }
        }

        private static void ShowMenu()
        {
            messageLogger.WriteMessageLine("Select an option: \n" +
                "1. Create order \n" +
                "2. Move to stock \n" +
                "3. Move to shipping \n" +
                "4. Change order status \n" +
                "5. Show specific order \n" +
                "6. Show all orders \n" +
                "7. Exit application");
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
