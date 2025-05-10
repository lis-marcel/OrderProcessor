using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Web.API.Controllers
{
    [ApiController]
    [Route("api")]
    [EnableCors("AllowVueApp")]
    public class OrderController : ControllerBase
    {
        private readonly DbStorage _dbStorageContext;

        public OrderController(DbStorage dbStorageContext)
        {
            _dbStorageContext = dbStorageContext;
        }

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

        [HttpGet("orders/{orderId}")]
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

        [HttpPost("orders")]
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

        [HttpGet("delete/{orderId}")]
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