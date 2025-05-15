using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Web.API.CommunicationModels;
using System.Security.Claims;

namespace OrderProcessor.Web.API.Controllers
{
    [Route("api/customer")]
    [ApiController]
    [EnableCors("AllowVueApp")]
    public class CustomersController : ControllerBase
    {
        private readonly DbStorage _dbStorageContext;

        public CustomersController(DbStorage dbStorageContext)
        {
            _dbStorageContext = dbStorageContext;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireCustomerRole")]
        [HttpPost("my-orders")]
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

    }
}
