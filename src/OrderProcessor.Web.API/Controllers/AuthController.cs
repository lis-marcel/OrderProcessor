using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;
using OrderProcessor.Web.API.CommunicationModels;

namespace OrderProcessor.Web.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [EnableCors("AllowVueApp")]
    public class AuthController : ControllerBase
    {
        private readonly DbStorage _dbStorageContext;
        private readonly TokenService _tokenService;
        private readonly AuthService _authService;
        private readonly CustomerService _customerService;

        public AuthController(
            DbStorage dbStorageContext,
            TokenService tokenService,
            AuthService authService,
            CustomerService customerService)
        {
            _dbStorageContext = dbStorageContext;
            _tokenService = tokenService;
            _authService = authService;
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationDto customerRegistrationData)
        {
            if (customerRegistrationData == null)
            {
                return BadRequest("User creation data is null.");
            }

            var result = await _customerService.RegisterCustomer(customerRegistrationData);

            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginCustomer([FromBody] CustomerLoginDto customerLoginData)
        {
            if (customerLoginData == null)
            {
                return BadRequest("User login data is null.");
            }

            var result = await _customerService.LoginCustomer(customerLoginData);

            if (result.Success && result.Data != null)
            {
                return Ok(result.Data);
            }
            else
            {
                return StatusCode(500, result.Message ?? "An error occurred while logging in the customer.");
            }
        }

    }
}
