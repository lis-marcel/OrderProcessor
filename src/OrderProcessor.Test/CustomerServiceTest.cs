using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.Test
{
    public class CustomerServiceTest
    {
        private static readonly Customer customer = new()
        {
            Id = 1,
            Name = "TestCustomer",
            CustomerType = CustomerType.Company,
        };

        // Due to the nature of the methods in CustomerService, it is not possible to test them without user input.

        //[Fact]
        //public void Test_AddCustomer()
        //{
        //    // Arrange
        //    var options = new DbContextOptionsBuilder<DbStorage>()
        //        .UseInMemoryDatabase(databaseName: "OrderTestDb")
        //        .Options;
        //    using var dbContext = new DbStorage(options);
        //    try
        //    {
        //        // Act
        //        //CustomerService.AddCustomer(dbContext);
        //        dbContext.Customers.Add(customer);
        //        dbContext.SaveChanges();
        //        //var input = "Jan\n1\n";
        //        //Console.SetIn(new StringReader(input));

        //        // Assert
        //        Assert.Equal(1, dbContext.Customers.Count());
        //    }
        //    finally
        //    {
        //        dbContext.Database.EnsureDeleted();
        //    }
        //}

        //[Fact]
        //public void Test_ValidateCustomerId_InvalidInput()
        //{
        //    // Arrange
        //    var options = new DbContextOptionsBuilder<DbStorage>()
        //        .UseInMemoryDatabase(databaseName: "OrderTestDb")
        //        .Options;
        //    using var dbContext = new DbStorage(options);
        //    try
        //    {
        //        dbContext.Customers.Add(customer);
        //        dbContext.SaveChanges();

        //        // Act
        //        int result = CustomerService.ValidateCustomerId(dbContext, 2);

        //        // Assert
        //        Assert.Equal(-1, result);
        //    }
        //    finally
        //    {
        //        dbContext.Database.EnsureDeleted();
        //    }
        //}

        [Fact]
        public void Test_ValidateCustomerId_ValidInput()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorage>()
                .UseInMemoryDatabase(databaseName: "OrderTestDb")
                .Options;

            using var dbContext = new DbStorage(options);
            try
            {
                dbContext.Customers.Add(customer);
                dbContext.SaveChanges();

                // Act
                int result = CustomerService.ValidateCustomerId(dbContext, 1);

                // Assert
                Assert.Equal(1, result);
            }
            finally
            {
                dbContext.Database.EnsureDeleted();
            }
        }
    }
}
