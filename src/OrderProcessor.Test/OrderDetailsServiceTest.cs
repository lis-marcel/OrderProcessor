//using OrderProcessor.BO;
//using OrderProcessor.BO.OrderOptions;
//using OrderProcessor.Common;
//using OrderProcessor.Service.DTO;
//using Microsoft.EntityFrameworkCore;
//using Xunit;
//using OrderProcessor.Console.Service;

//namespace OrderProcessor.Service.Test
//{
//    public class OrderDetailsServiceTest
//    {
//        private static readonly ConsoleLogger consoleLogger = new();

//        [Fact]
//        public void Test_GetDoubleValue_ValidInput()
//        {
//            // Arrange
//            var input = "12,34";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            double result = UserInputHandler.EnterDoubleValue("TestField", consoleLogger);

//            // Assert
//            Assert.Equal(12.34, result);
//        }

//        [Fact]
//        public void Test_GetDoubleValue_InvalidInputFollowedByValid()
//        {
//            // Arrange
//            var input = "abc\n12,34";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            double result = UserInputHandler.EnterDoubleValue("TestField", consoleLogger);

//            // Assert
//            Assert.Equal(12.34, result);
//        }

//        [Fact]
//        public void Test_GetStringValue_ValidInput()
//        {
//            // Arrange
//            var input = "TestValue";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            string result = UserInputHandler.EnterStringValue("TestField", consoleLogger);

//            // Assert
//            Assert.Equal("TestValue", result);
//        }

//        [Fact]
//        public void Test_GetStringValue_EmptyInputFollowedByValid()
//        {
//            // Arrange
//            var input = "\nTestValue";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            string result = UserInputHandler.EnterStringValue("TestField", consoleLogger);

//            // Assert
//            Assert.Equal("TestValue", result);
//        }

//        [Fact]
//        public void Test_GetIntValue_ValidInput()
//        {
//            // Arrange
//            var input = "42";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            int result = UserInputHandler.EnterIntValue("TestField", consoleLogger);

//            // Assert
//            Assert.Equal(42, result);
//        }

//        [Fact]
//        public void Test_GetIntValue_InvalidInputFollowedByValid()
//        {
//            // Arrange
//            var input = "abc\n42";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            int result = UserInputHandler.EnterIntValue("TestField", consoleLogger);
                
//            // Assert
//            Assert.Equal(42, result);
//        }

//        [Fact]
//        public void Test_GetCustomerType_ValidInput()
//        {
//            // Arrange
//            var input = "1";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            CustomerType result = OrderDetalisService.EnterCustomerType(consoleLogger);

//            // Assert
//            Assert.Equal(CustomerType.Company, result);
//        }

//        [Fact]
//        public void Test_GetCustomerType_InvalidInputFollowedByValid() 
//        {
//            // Arrange
//            var input = "abc\n998\n2";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            CustomerType result = OrderDetalisService.EnterCustomerType(consoleLogger);

//            // Assert
//            Assert.Equal(CustomerType.Individual, result);
//        }

//        [Fact]
//        public void Test_GetPaymentMethod_ValidInput()
//        {
//            // Arrange
//            var input = "1";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            PaymentMethod result = OrderDetalisService.EnterPaymentMethod(consoleLogger);

//            // Assert
//            Assert.Equal(PaymentMethod.Cash, result);
//        }

//        [Fact]
//        public void Test_GetPaymentMethod_InvalidInputFollowedByValid()
//        {
//            // Arrange
//            var input = "90\n1";
//            System.Console.SetIn(new StringReader(input));

//            // Act
//            PaymentMethod result = OrderDetalisService.EnterPaymentMethod(consoleLogger);

//            // Assert
//            Assert.Equal(PaymentMethod.Cash, result);
//        }

//    }
//}
