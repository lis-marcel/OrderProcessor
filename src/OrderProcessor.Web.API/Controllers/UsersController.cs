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
    public class UsersController : ControllerBase
    {
        private readonly DbStorage _dbStorageContext;
        private readonly TokenService _tokenService;
        private readonly AuthService _authService;
        private readonly CustomerService _customerService;

        public UsersController(DbStorage dbStorageContext,
            TokenService tokenService,
            AuthService authService,
            CustomerService customerService)
        {
            _dbStorageContext = dbStorageContext;
            _tokenService = tokenService;
            _authService = authService;
            _customerService = customerService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireCustomerRole")]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireCustomerRole")]
        [HttpPost("my-profile")]
        public IActionResult CustomerProfile([FromBody] ProtectedRequest protectedRequest)
        {
            try
            {
                // Get user email from claims
                var email = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var orders = CustomerService.GetCustomerData(_dbStorageContext, email);

                if (orders == null)
                {
                    return NotFound($"Customer with email {email} not found.");
                }

                return Ok(orders);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving customer orders.");
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] UserChangePasswordDto newPasswordDto)
        {
            try
            {
                // Get user email from claims
                var email = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var result = await _customerService.ChangePassword(newPasswordDto);

                if (result == null)
                {
                    return NotFound($"Customer with email {email} not found.");
                }

                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving customer orders.");
            }
        }

        [Authorize]
        [HttpPost("edit-profile")]
        public async Task<IActionResult> EditUserProfile([FromBody] EditUserDto editUserDto)
        {
            try
            {
                // Get user email from claims
                var email = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var result = await _customerService.UpdateCustomerData(editUserDto);

                if (result == null)
                {
                    return NotFound($"Customer with email {email} not found.");
                }

                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving customer orders.");
            }
        }

    }
}
