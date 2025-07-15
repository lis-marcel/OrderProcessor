using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Web.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [EnableCors("AllowVueApp")]
    public class OrdersController : ControllerBase
    {
        private readonly DbStorage _dbStorageContext;

        public OrdersController(DbStorage dbStorageContext)
        {
            _dbStorageContext = dbStorageContext;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdminOrCustomerRole")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreationDto orderCreationData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await OrderService.CreateOrder(_dbStorageContext, orderCreationData);

            if (result.Success)
            {
                return Ok("Order created successfully.");
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireCustomerRole")]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await OrderService.GetOrder(_dbStorageContext, orderId);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdministratorRole")]
        [HttpGet("all-orders")]
        public IActionResult GetOrders()
        {
            var result = OrderService.GetOrders(_dbStorageContext);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdministratorRole")]
        [HttpDelete("delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await OrderService.DeleteOrder(_dbStorageContext, orderId);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }

    }
}