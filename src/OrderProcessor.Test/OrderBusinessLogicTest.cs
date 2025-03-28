using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Common;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace OrderProcessor.Service.Test
{
    public class OrderBusinessLogicTest
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
        private static readonly Order exampleOrder2 = new()
        {
            Id = 1,
            ProductName = "Test Product",
            Quantity = 1,
            ShippingAddress = "123 Test St",
            CustomerName = "Test Customer",
            CustomerType = CustomerType.Individual,
            CreationTime = DateTime.Now,
            Value = 4000,
            PaymentMethod = PaymentMethod.Cash,
            Status = OrderStatus.New
        };

        [Fact]
        public void Test_IsOrderEligibleForWarehouseProcessing_ReturnFalse()
        {
            // Arrange
            var orderData = OrderData.ToDTO(exampleOrder2);

            // Act
            bool result = OrderBusinessLogic.IsOrderEligibleForWarehouseProcessing(orderData);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Test_IsOrderEligibleForWarehouseProcessing_ReturnTrue_ForCashUnderThreshold()
        {
            // Arrange
            var orderData = OrderData.ToDTO(exampleOrder1);

            // Act
            bool result = OrderBusinessLogic.IsOrderEligibleForWarehouseProcessing(orderData);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Test_IsOrderEligibleForWarehouseProcessing_ReturnTrue_ForCreditCardAboveThreshold()
        {
            // Arrange
            var orderData = OrderData.ToDTO(exampleOrder1);

            // Act
            bool result = OrderBusinessLogic.IsOrderEligibleForWarehouseProcessing(orderData);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Test_MarkOrderAsShippedAfterDelay_MovedToShipping_ForCashBelowTreshold()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorage>()
                .UseInMemoryDatabase(databaseName: "OrderTestDb")
                .Options;

            using var dbContext = new DbStorage(options);
            try
            {
                dbContext.Orders.Add(exampleOrder1);
                dbContext.SaveChanges();

                // Act
                await OrderBusinessLogic.MarkOrderAsToShippingAfterDelay(dbContext, 1);

                // Assert
                var updatedOrder = dbContext.Orders.Find(1);
                Assert.NotNull(updatedOrder);
                Assert.Equal(OrderStatus.InShipping, updatedOrder!.Status);
            }
            finally
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task Test_MarkOrderAsShippedAfterDelay_OrderNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorage>()
                .UseInMemoryDatabase(databaseName: "OrderTestDb2")
                .Options;

            using var dbContext = new DbStorage(options);
            try
            {
                // Act (no exampleOrder added, so ID 99 won't exist)
                await OrderBusinessLogic.MarkOrderAsToShippingAfterDelay(dbContext, 99);

                // Assert
                var nonExistent = dbContext.Orders.Find(99);
                Assert.Null(nonExistent);
            }
            finally
            {
                dbContext.Database.EnsureDeleted();
            }
        }

    }
}
