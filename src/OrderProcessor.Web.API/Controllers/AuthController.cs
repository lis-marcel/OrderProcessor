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

        public AuthController(
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
                return BadRequest("User creation data is null.");
            }

            var result = await CustomerService.RegisterCustomer(_dbStorageContext, customerRegistrationData);

            if (result)
            {
                return Ok("User created successfully.");
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
                return BadRequest("User login data is null.");
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
                    CustomerData = UserData.ToDTO(result.Item2)
                };

                return Ok(loginResponse);
            }
            else
            {
                return StatusCode(500, "An error occurred while loggin in the customer.");
            }

        }

    }
}
