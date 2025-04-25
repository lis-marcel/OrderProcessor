using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.BO.OrderOptions;
using OrderProcessor.Console.Service;
using OrderProcessor.Service.DTO;
using OrderProcessor.Web.API.BO;

namespace OrderProcessor.Web.API.Controllers;

[ApiController]
[Route("/order")]
public class OrderController : ControllerBase
{
    [HttpPost]
    [Route("/create")]
    public IActionResult CreateOrder(DbStorage dbContext, [FromBody]OrderRequest orderRequest)
    {
        try
        {
            if (orderRequest == null)
            {
                return BadRequest("Order data is null");
            }

            var orderData = new OrderData
            {
                Value = orderRequest.Value,
                ProductName = orderRequest.ProductName,
                ShippingAddress = orderRequest.ShippingAddress,
                Status = OrderStatus.New,
                Quantity = orderRequest.Quantity,
                CreationTime = DateTime.Now,
                MarkToShippingAt = null,
                CustomerId = orderRequest.CustomerId,
                PaymentMethod = orderRequest.PaymentMethod
            };

            dbContext.Orders.Add(OrderData.ToBO(orderData));
            dbContext.SaveChanges();

            int newOrderId = DbStorageService.GetHighestId<Order>(dbContext);

            return Ok($"Order created successfully with ID: {newOrderId}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
