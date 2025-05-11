using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.DTO;

namespace OrderProcessor.Web.API.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors("AllowVueApp")]
    public class AuthController : ControllerBase
    {
        private readonly DbStorage _dbStorageContext;

        public AuthController(DbStorage dbStorageContext)
        {
            _dbStorageContext = dbStorageContext;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationData customerRegistrationData)
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
    }
}
