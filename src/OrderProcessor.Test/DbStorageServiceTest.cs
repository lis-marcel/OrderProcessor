using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace OrderProcessor.Service.Test
{
    public class DbStorageServiceTest
    {
        private static readonly Order exampleOrder = new()
        {
            Id = 1,
            ProductName = "Test Product",
            Quantity = 1,
            ShippingAddress = "123 Test St",
            CustomerId = 1,
            CreationTime = DateTime.Now,
            Value = 1000,
            PaymentMethod = PaymentMethod.CreditCard,
            Status = OrderStatus.New
        };
        private static readonly Customer exampleCustomer = new()
        {
            Name = "TestCustomer",
            CustomerType = CustomerType.Company,
        };

        [Fact]
        public void Test_GetOrderById_ReturnsExistingOrderId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorage>()
                .UseInMemoryDatabase(databaseName: "Test_Dabatabase1")
                .Options;

            using var dbContext = new DbStorage(options);
            try
            {
                dbContext.Orders.Add(exampleOrder);
                dbContext.SaveChanges();

                // Act
                int highestId = DbStorageService.GetHighestId<Order>(dbContext);

                // Assert
                Assert.Equal(1, highestId);
            }
            finally
            {
                dbContext.Database.EnsureDeleted();
            }                       
        }

        [Fact]
        public void Test_GetOrderById_EmptyDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorage>()
                .UseInMemoryDatabase(databaseName: "Test_Dabatabase2")
                .Options;

            using var dbContext = new DbStorage(options);
            try
            {
                // Act
                int highestId = DbStorageService.GetHighestId<Order>(dbContext);

                // Assert
                Assert.Equal(0, highestId);
            }
            finally
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void Test_GetCustomerById_ReturnsExistingCustomerId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorage>()
                .UseInMemoryDatabase(databaseName: "Test_Dabatabase3")
                .Options;
            using var dbContext = new DbStorage(options);
            try
            {
                dbContext.Customers.Add(exampleCustomer);
                dbContext.SaveChanges();

                // Act
                int highestId = DbStorageService.GetHighestId<Customer>(dbContext);

                // Assert
                Assert.Equal(1, highestId);
            }
            finally
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void Test_GetCustomerById_EmptyDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorage>()
                .UseInMemoryDatabase(databaseName: "Test_Dabatabase4")
                .Options;
            using var dbContext = new DbStorage(options);
            try
            {
                // Act
                int highestId = DbStorageService.GetHighestId<Customer>(dbContext);

                // Assert
                Assert.Equal(0, highestId);
            }
            finally
            {
                dbContext.Database.EnsureDeleted();
            }
        }

    }
}
