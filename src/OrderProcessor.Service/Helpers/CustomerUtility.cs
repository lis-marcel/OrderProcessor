using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service.Helpers
{
    public class CustomerUtility
    {
        public static UserDto CreateCustomerDetails(CustomerRegistrationDto customerCreationData)
        {
            return new()
            {
                Name = customerCreationData.Name,
                Email = customerCreationData.Email!,
                Password = customerCreationData.Password!,
                LastLoginAt = DateTime.Now,
                CustomerType = customerCreationData.CustomerType
            };
        }
    }
}
