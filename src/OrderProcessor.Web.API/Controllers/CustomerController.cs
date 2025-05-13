using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;
using OrderProcessor.Web.API.CommunicationModels;

namespace OrderProcessor.Web.API.Controllers
{
    [Route("api/customer")]
    [ApiController]
    [EnableCors("AllowVueApp")]
    public class CustomerController : ControllerBase
    {
        private readonly DbStorage _dbStorageContext;

        public CustomerController(DbStorage dbStorageContext)
        {
            _dbStorageContext = dbStorageContext;
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

            var result = await CustomerService.LoginCustomer(_dbStorageContext, customerLoginData);

            if (result.Item1.HasValue && result.Item2 != null)
            {
                LoginResponse loginResponse = new()
                {
                    SessionToken = result.Item1.Value,
                    CustomerData = CustomerData.ToDTO(result.Item2)
                };

                return Ok(loginResponse);
            }
            else
            {
                return StatusCode(500, "An error occurred while loggin in the customer.");
            }

        }

        [HttpPost("customer-orders")]
        public async Task<IActionResult> CustomerOrders([FromBody] ProtectedRequest protectedRequest)
        {
            // Extract the Bearer token from the Authorization header
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return BadRequest("Authorization header is missing.");
            }

            string authHeaderValue = authHeader.ToString();

            // Check if it's a Bearer token
            if (!authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Authorization header must start with 'Bearer '.");
            }

            // Extract the token part
            string tokenString = authHeaderValue.Substring("Bearer ".Length).Trim();

            // Parse the token to GUID
            if (!Guid.TryParse(tokenString, out var sessionToken))
            {
                return BadRequest("Invalid session token format.");
            }

            if (protectedRequest == null)
            {
                return BadRequest("Customer login data is null.");
            }

            var result = AuthService.AuthCustomer(_dbStorageContext, sessionToken);

            if (result.Item1 == null || result.Item2 == null)
            {
                return StatusCode(500, "An error occurred while loggin in the customer.");
            }
            else
            {
                var orders = await CustomerService.GetCustomerOrders(_dbStorageContext, protectedRequest.CustomerEmail!);

                return Ok(orders);
            }

        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthCustomer([FromBody] Guid sessionToken)
        {
            if (sessionToken == Guid.Empty)
            {
                return BadRequest("Session token is null.");
            }

            var result = AuthService.AuthCustomer(_dbStorageContext, sessionToken);

            if (result.Item1)
            {
                return Ok(result.Item2);
            }
            else
            {
                return StatusCode(500, "An error occurred while authenticating the customer.");
            }
        }

    }
}
