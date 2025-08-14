using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service.Helpers
{
    public class CustomerUtility
    {
        public static UserAllDataDto CreateCustomerDetails(CustomerRegistrationDto customerCreationData)
        {
            return new()
            {
                Name = customerCreationData.Name,
                Email = customerCreationData.Email!,
                Password = PasswordFunctionalities.HashPassword(customerCreationData.Password!, out var salt),
                Salt = salt,
                LastLoginAt = DateTime.Now,
                CustomerType = customerCreationData.CustomerType,
                AccountType = BO.OrderOptions.AccountType.Customer, 
            };
        }
    }
}
