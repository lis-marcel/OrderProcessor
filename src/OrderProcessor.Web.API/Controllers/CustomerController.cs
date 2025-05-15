using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;
using OrderProcessor.Web.API.CommunicationModels;
using System.Security.Claims;

namespace OrderProcessor.Web.API.Controllers
{
    [Route("api/customer")]
    [ApiController]
    [EnableCors("AllowVueApp")]
    public class CustomerController : ControllerBase
    {
        private readonly DbStorage _dbStorageContext;
        private readonly TokenService _tokenService;
        private readonly AuthService _authService;

        public CustomerController(
            DbStorage dbStorageContext,
            TokenService tokenService,
            AuthService authService)
        {
            _dbStorageContext = dbStorageContext;
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationData customerRegistrationData)
        {
            if (customerRegistrationData == null)
            {
                return BadRequest("Customer creation data is null.");
            }

            var result = await CustomerService.RegisterCustomer(_dbStorageContext, customerRegistrationData);

            if (result)
            {
                return Ok("Customer created successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while creating the customer.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginCustomer([FromBody] CustomerLoginData customerLoginData)
        {
            if (customerLoginData == null)
            {
                return BadRequest("Customer login data is null.");
            }

            var result = await CustomerService.LoginCustomer(
                _dbStorageContext,
                customerLoginData,
                _tokenService);

            if (!string.IsNullOrEmpty(result.Item1) && result.Item2 != null)
            {
                LoginResponse loginResponse = new()
                {
                    Token = result.Item1,
                    CustomerData = CustomerData.ToDTO(result.Item2)
                };

                return Ok(loginResponse);
            }
            else
            {
                return StatusCode(500, "An error occurred while loggin in the customer.");
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("customer-orders")]
        public IActionResult CustomerOrders([FromBody] ProtectedRequest protectedRequest)
        {
            try
            {
                // Get user email from claims
                var email = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var orders = CustomerService.GetCustomerOrders(_dbStorageContext, email);

                return Ok(orders);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving customer orders.");
            }
        }

    }
}
