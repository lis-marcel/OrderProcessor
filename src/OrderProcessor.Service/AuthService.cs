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
                    return OperationResult.Failed("Invalid email or password.");
                }

                existingCustomer.LastLoginAt = DateTime.Now;
                _dbContext.SaveChanges();

                return OperationResult.Succeeded("User authenticated successfully.", existingCustomer);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"Error occurred during authenticating the customer: {ex.Message}");
            }
        }

        public OperationResult ValidateToken(string token)
        {
            try
            {
                var claims = _tokenService.ValidateToken(token);
                if (claims == null)
                {
                    return OperationResult.Failed("Invalid token.");
                }

                var emailClaim = claims.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(emailClaim))
                {
                    return OperationResult.Failed("Token does not contain valid email claim.");
                }

                var customer = _dbContext.Customers
                    .FirstOrDefault(c => c.Email == emailClaim);

                if (customer == null)
                {
                    return OperationResult.Failed("Customer not found for the provided token.");
                }

                return OperationResult.Succeeded("Token validated successfully.", customer);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"Error occurred during token validation: {ex.Message}");
            }
        }
    }
}
