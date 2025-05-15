// OrderProcessor.Service/AuthService.cs
using Microsoft.Extensions.Options;
using OrderProcessor.BO;
using OrderProcessor.BO.Entities;
using OrderProcessor.Service.Auth;
using System.Security.Claims;

namespace OrderProcessor.Service
{
    public class AuthService
    {
        private readonly TokenService _tokenService;

        public AuthService(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // This method is just a simplified example of how to authenticate a customer. 
        public static (bool, Customer) AuthenticateCustomer(DbStorage dbStorage, string email, string password)
        {
            try
            {
                var existingCustomer = dbStorage.Customers
                    .Where(c => c.Email == email && c.Password == password)
                    .FirstOrDefault();

                if (existingCustomer == null)
                {
                    return (false, null);
                }

                existingCustomer.LastLoginAt = DateTime.Now;
                dbStorage.SaveChanges();

                return (true, existingCustomer);
            }
            catch
            {
                return (false, null);
            }
        }

        public (bool, Customer) ValidateToken(DbStorage dbStorage, string token)
        {
            try
            {
                var claims = _tokenService.ValidateToken(token);
                if (claims == null)
                {
                    return (false, null);
                }

                var emailClaim = claims.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(emailClaim))
                {
                    return (false, null);
                }

                var customer = dbStorage.Customers
                    .FirstOrDefault(c => c.Email == emailClaim);

                if (customer == null)
                {
                    return (false, null);
                }

                return (true, customer);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
