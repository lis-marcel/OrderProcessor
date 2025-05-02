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
        private readonly OrderService _orderService;

        public OrderController(DbStorage dbStorageContext, OrderService orderService)
        {
            _dbStorageContext = dbStorageContext;
            _orderService = orderService;
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
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        [HttpGet("order/{orderId}")]
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
            var order = OrderService.GetOrders(_dbStorageContext);
            if (order != null)
            {
                return Ok(order);
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