using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Db;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace OrderProcessor.Service
{
    public class ConsoleService
    {
        private DatabaseService databaseService;
        private DbStorage dbStorageContext;

        public ConsoleService() 
        {
            dbStorageContext = new DbStorage(new DbContextOptionsBuilder<DbStorage>()
                .UseSqlite("Data Source=C:\\temp\\OrderProcessor.db").Options);

            databaseService = new DatabaseService(dbStorageContext);
        }

        #region Public Methods
        public void Start()
        {
            Console.WriteLine("Welcome to Order Processor\n" +
                "1. Create Order\n" +
                "2. Move to Stock\n" +
                "3. Move to Shipping\n" +
                "4. Show All Orders\n" +
                "5. Exit");

            string input = Console.ReadLine();

            Console.Clear();

            switch (input)
            {
                case "1":
                    CreateOrder();
                    break;
                case "2":
                    MoveToStock();
                    break;
                case "3":
                    MoveToShipping();
                    break;
                case "4":
                    ChangeOrderStatus();
                    break;
                case "5":
                    ShowAllOrders();
                    break;
                case "6":
                    ExitApp();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
        #endregion

        #region Private Methods
        private void CreateOrder()
        {
            throw new NotImplementedException();
        }

        private void MoveToStock()
        {
            throw new NotImplementedException();
        }

        private void MoveToShipping()
        {
            throw new NotImplementedException();
        }

        private void ChangeOrderStatus()
        {
            throw new NotImplementedException();
        }

        private void ShowAllOrders()
        {
            throw new NotImplementedException();
        }

        private void ExitApp()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
