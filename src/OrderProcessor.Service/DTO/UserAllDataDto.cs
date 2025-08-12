using OrderProcessor.BO.Entities;
using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.Service.DTO
{
    public class UserAllDataDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public byte[]? Salt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public CustomerType CustomerType { get; set; }
        public AccountType AccountType { get; set; }

        public static User ToBo(UserAllDataDto customerData)
        {
            return new User
            {
                Id = customerData.Id,
                Name = customerData.Name,
                Email = customerData.Email,
                Password = customerData.Password,
                Salt = customerData.Salt ?? [],
                LastLoginAt = customerData.LastLoginAt,
                CustomerType = customerData.CustomerType,
                AccountType = customerData.AccountType,
            };
        }

        public static UserAllDataDto ToDto(User customer)
        {
            return new UserAllDataDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                LastLoginAt = customer.LastLoginAt,
                CustomerType = (CustomerType)customer.CustomerType!,
                AccountType= customer.AccountType,
            };
        }

    }
}
