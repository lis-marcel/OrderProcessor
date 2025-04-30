//using Microsoft.EntityFrameworkCore;
//using OrderProcessor.BO;
//using OrderProcessor.BO.OrderOptions;
//using OrderProcessor.Common;
//using OrderProcessor.Console.Service;
//using OrderProcessor.Service.DTO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OrderProcessor.Service.Test
//{
//    public class CustomerServiceTest
//    {
//        private static readonly Customer customer1 = new()
//        {
//            Id = 1,
//            Name = "TestCustomer1",
//            CustomerType = CustomerType.Company,
//        };
//        private static readonly CustomerData customerData = new()
//        {
//            Name = "TestCustomer2",
//            CustomerType = CustomerType.Individual,
//        };

//        // Due to the nature of the methods in CustomerService, it is not possible to test them without user input.

//        //[Fact]
//        //public void Test_AddCustomer()
//        //{
//        //    // Arrange
//        //    var options = new DbContextOptionsBuilder<DbStorage>()
//        //        .UseInMemoryDatabase(databaseName: "OrderTestDb")
//        //        .Options;
//        //    using var dbContext = new DbStorage(options);
//        //    try
//        //    {
//        //        // Act
//        //        //CustomerService.CreateCustomer(dbContext);
//        //        dbContext.Customers.Add(customer);
//        //        dbContext.SaveChanges();
//        //        //var input = "Jan\n1\n";
//        //        //Console.SetIn(new StringReader(input));

//        //        // Assert
//        //        Assert.Equal(1, dbContext.Customers.Count());
//        //    }
//        //    finally
//        //    {
//        //        dbContext.Database.EnsureDeleted();
//        //    }
//        //}

//        //[Fact]
//        //public void Test_ValidateCustomerId_InvalidInput()
//        //{
//        //    // Arrange
//        //    var options = new DbContextOptionsBuilder<DbStorage>()
//        //        .UseInMemoryDatabase(databaseName: "OrderTestDb")
//        //        .Options;
//        //    using var dbContext = new DbStorage(options);
//        //    try
//        //    {
//        //        dbContext.Customers.Add(customer);
//        //        dbContext.SaveChanges();

//        //        // Act
//        //        int result = CustomerService.ValidateCustomerId(dbContext, 2);

//        //        // Assert
//        //        Assert.Equal(-1, result);
//        //    }
//        //    finally
//        //    {
//        //        dbContext.Database.EnsureDeleted();
//        //    }
//        //}

//        [Fact]
//        public void Test_ValidateCustomerId_ValidInput_Success()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<DbStorage>()
//                .UseInMemoryDatabase(databaseName: "OrderTestDb")
//                .Options;

//            using var dbContext = new DbStorage(options);
//            try
//            {
//                dbContext.Customers.Add(customer1);
//                dbContext.SaveChanges();

//                // Act
//                int result = CustomerService.ValidateCustomerId(dbContext, 1);

//                string input = "1\n";
//                System.Console.SetIn(new StringReader(input));

//                // Assert
//                Assert.Equal(1, result);
//            }
//            finally
//            {
//                dbContext.Database.EnsureDeleted();
//            }
//        }

//        [Fact]
//        public void Test_ValidateCustomerId_InvalidInputFollowedByValid_Success()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<DbStorage>()
//                .UseInMemoryDatabase(databaseName: "OrderTestDb")
//                .Options;

//            using var dbContext = new DbStorage(options);
//            try
//            {
//                dbContext.Customers.Add(customer1);
//                dbContext.SaveChanges();

//                // Act
//                int result = CustomerService.ValidateCustomerId(dbContext, 1);

//                string input = "abc\n1\n";
//                System.Console.SetIn(new StringReader(input));

//                // Assert
//                Assert.Equal(1, result);
//            }
//            finally
//            {
//                dbContext.Database.EnsureDeleted();
//            }
//        }

//        [Fact]
//        public void Test_EditCustomer_ValidInput_Success()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<DbStorage>()
//                .UseInMemoryDatabase(databaseName: "OrderTestDb")
//                .Options;

//            using var dbContext = new DbStorage(options);
//            try
//            {
//                dbContext.Customers.Add(customer1);
//                dbContext.SaveChanges();
//                // Act

//                var retrievedCustomer = dbContext.Customers.First();

//                // Implementation of EditCustomer method
//                var updatedCustomerData = CustomerData.ToBO(customerData);
//                retrievedCustomer = updatedCustomerData; 
//                retrievedCustomer.Id = customer1.Id; // Ensure the ID remains the same

//                dbContext.SaveChanges();

//                // Assert
//                Assert.Equal("TestCustomer2", retrievedCustomer.Name);
//            }
//            finally
//            {
//                dbContext.Database.EnsureDeleted();
//            }
//        }

//        [Fact]
//        public void Test_DeleteCustomer_Success()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<DbStorage>()
//                .UseInMemoryDatabase(databaseName: "OrderTestDb")
//                .Options;

//            using var dbContext = new DbStorage(options);
//            try
//            {
//                dbContext.Customers.Add(customer1);
//                dbContext.SaveChanges();

//                // Act
//                CustomerService.DeleteCustomer(dbContext, 1);
//                dbContext.SaveChanges();

//                // Assert
//                Assert.Empty(dbContext.Customers);
//            }
//            finally
//            {
//                dbContext.Database.EnsureDeleted();
//            }
//        }

//    }
//}
