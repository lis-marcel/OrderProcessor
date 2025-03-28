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
        private static readonly Order exampleOrder1 = new()
        {
            Id = 1,
            ProductName = "Test Product",
            Quantity = 1,
            ShippingAddress = "123 Test St",
            CustomerName = "Test Customer",
            CustomerType = CustomerType.Individual,
            CreationTime = DateTime.Now,
            Value = 1000,
            PaymentMethod = PaymentMethod.CreditCard,
            Status = OrderStatus.New
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
                dbContext.Orders.Add(exampleOrder1);
                dbContext.SaveChanges();

                // Act
                int highestId = DbStorageService.GetHighestOrderId(dbContext);

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
                int highestId = DbStorageService.GetHighestOrderId(dbContext);

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
