using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;
using OrderProcessor.Web.API.Auth;

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
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreationData orderCreationData)
        {
            if (orderCreationData == null)
            {
                return BadRequest("Order creation data is null.");
            }

            var result = await OrderService.CreateOrder(_dbStorageContext, orderCreationData);

            if (result)
            {
                return Ok("Order created successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while creating the orders.");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireCustomerRole")]
        [HttpGet("{orderId}")]
        public IActionResult GetOrder(int orderId)
        {
            var order = OrderService.GetOrder(_dbStorageContext, orderId);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound($"Order with ID {orderId} not found.");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdministratorRole")]
        [HttpPost("all-orders")]
        public IActionResult GetOrders()
        {
            var orders = OrderService.GetOrders(_dbStorageContext);
            if (orders != null)
            {
                return Ok(orders);
            }
            else
            {
                return NotFound($"No orders found.");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RequireAdministratorRole")]
        [HttpDelete("delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await OrderService.DeleteOrder(_dbStorageContext, orderId);
            if (order == true)
            {
                return Ok(order);
            }
            else
            {
                return NotFound($"Order with ID {orderId} not found.");
            }
        }

    }
}