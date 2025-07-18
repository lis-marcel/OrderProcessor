﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.BO.Entities;
using OrderProcessor.Common;
using OrderProcessor.Service.DTO;
using OrderProcessor.Service.Helpers;

namespace OrderProcessor.Service
{
    public class CustomerService
    {
        private readonly DbStorage _dbContext;
        private readonly AuthService _authService;
        private readonly TokenService _tokenService;

        public CustomerService(DbStorage dbContext, AuthService authService, TokenService tokenService) 
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<OperationResult> RegisterCustomer(CustomerRegistrationDto customerRegistrationDto)
        {
            if (string.IsNullOrEmpty(customerRegistrationDto.Email) ||
                string.IsNullOrEmpty(customerRegistrationDto.Name) ||
                string.IsNullOrEmpty(customerRegistrationDto.Password)) 
            {
                return OperationResult.Failed("Username, password, and email are required.");
            }

            var emailExists = await EmailExists(customerRegistrationDto.Email);

            if (emailExists)
            {
                return OperationResult.Failed("Email already in use.");
            }

            try
            { 
                var customerData = CustomerUtility.CreateCustomerDetails(customerRegistrationDto);

                await _dbContext.Customers.AddAsync(UserDto.ToBo(customerData));
                await _dbContext.SaveChangesAsync();

                return OperationResult.Succeeded("User created successfully.", customerData);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"Error occured during creating the user: {ex.Message}");
            }
        }

        public async Task<OperationResult> LoginCustomer(CustomerLoginDto customerLoginData)
        {
            if (string.IsNullOrEmpty(customerLoginData.Email) ||
                string.IsNullOrEmpty(customerLoginData.Password))
            {
                return OperationResult.Failed("Password and email are required.");
            }

            try
            {
                var authResult = _authService.AuthenticateCustomer(
                    customerLoginData.Email,
                    customerLoginData.Password);

                if (!authResult.Success)
                {
                    return OperationResult.Failed("Failed to authenticate customer.");
                }

                // Generate JWT token
                var user = authResult.Data as User;

                if (user == null)
                {
                    return OperationResult.Failed("Invalid user data.");
                }
                var token = _tokenService.GenerateJwtToken(user);

                if (string.IsNullOrEmpty(token))
                {
                    return OperationResult.Failed("Failed to generate authentication token.");
                }

                authResult.Data = DateTime.Now;
                await _dbContext.SaveChangesAsync();

                // Return login data with both token and user info
                var loginData = new
                {
                    Token = token,
                    User = UserDto.ToDto(user)
                };

                return OperationResult.Succeeded("Login successful", loginData);
            }
            catch (Exception ex)
            {
                return OperationResult.Failed($"Error occured during creating the user: {ex.Message}");
            }
        }

        public static List<Order> GetCustomerOrders(DbStorage dbStorageContext, string email)
        {
            try
            {
                var customer = dbStorageContext.Customers
                    .Where(c => c.Email == email)
                    .AsNoTracking()
                    .FirstOrDefault();

                if (customer == null)
                {
                    return new List<Order>();
                }

                var orders = dbStorageContext.Orders
                    .Where(o => o.CustomerId == customer.Id)
                    .ToList();

                return orders;
            }
            catch
            {
                return new List<Order>();
            }
        }

        public static UserDto? GetCustomerData(DbStorage dbStorageContext, string email)
        {
            try
            {
                var user = dbStorageContext.Customers
                    .Where(c => c.Email == email)
                    .AsNoTracking()
                    .FirstOrDefault();

                if (user == null)
                {
                    return null;
                }

                return UserDto.ToDto(user);
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> EmailExists(string email)
        {
            var result = await _dbContext.Customers
                .FirstOrDefaultAsync(u => u.Email == email);

            return true && result != null;
        }

    }
}
