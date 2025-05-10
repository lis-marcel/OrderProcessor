using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;

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

            if (result.HasValue)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, "An error occurred while loggin in the customer.");
            }

        }
    }
}
