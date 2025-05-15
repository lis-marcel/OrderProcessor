using OrderProcessor.Service.DTO;

namespace OrderProcessor.Service
{
    public class CustomerUtility
    {
        public static UserData CreateCustomerDetails(CustomerRegistrationData customerCreationData)
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
