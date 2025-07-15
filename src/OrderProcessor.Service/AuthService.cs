// OrderProcessor.Service/AuthService.cs
using Microsoft.Extensions.Options;
using OrderProcessor.BO;
using OrderProcessor.BO.Entities;
using OrderProcessor.Common;
using OrderProcessor.Service.Auth;
using System.Security.Claims;

namespace OrderProcessor.Service
{
    public class AuthService
    {
        private readonly DbStorage _dbContext;
        private readonly TokenService _tokenService;

        public AuthService(DbStorage dbContext, TokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        // This method is just a simplified example of how to authenticate a customer. 
        public OperationResult AuthenticateCustomer(string email, string password)
        {
            try
            {
                var existingCustomer = _dbContext.Customers
                    .Where(c => c.Email == email && c.Password == password)
                    .FirstOrDefault();

                if (existingCustomer == null)
                {
                    return OperationResult.Failed();
                }

                existingCustomer.LastLoginAt = DateTime.Now;
                _dbContext.SaveChanges();

                return OperationResult.Succeeded("User found.", existingCustomer);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"Error occured during authenticationg the customer: {ex.Message}");
            }
        }

        public (bool, User) ValidateToken(DbStorage dbStorage, string token)
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
