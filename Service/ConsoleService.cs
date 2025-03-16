using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Service.DTO;
using Microsoft.EntityFrameworkCore.Sqlite;
using OrderProcessor.OrderOptions;

namespace OrderProcessor.Service
{
    internal class ConsoleService
    {
        private readonly DbStorage dbStorageContext;

        public ConsoleService() 
        {
            var dbPath = DbStorageService.PrepareDb();

            dbStorageContext = 
                new DbStorage(new DbContextOptionsBuilder<DbStorage>()
                .UseSqlite($"Data Source={dbPath}")
                .Options);
        }

        #region Public Methods
        public void Start()
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
                    OrderManagmentService.ShowAllOrders(dbStorageContext);
                    break;
                case "6":
                    ExitApp();
                    break;
            }
        }
        #endregion

        #region Private Methods
        private static string SelectedOption()
        {
            while (true)
            {
                ShowMenu();

                Console.Write("Enter option number: ");
                if (int.TryParse(Console.ReadLine(), out int value) && value >= 0 && value < Enum.GetNames(typeof(Operation)).Length)
                {
                    return value.ToString();
                }
                Console.Clear();
                Console.WriteLine("Invalid input. Please enter a correct value.");
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("Welcome to Order Processor\n" +
                "1. Create Order\n" +
                "2. Move to Stock\n" +
                "3. Move to Shipping\n" +
                "4. Show All Orders\n" +
                "5. Exit");
        }
                        
        private void ExitApp()
        {
            throw new NotImplementedException();
        }

        

        #endregion

    }
}
