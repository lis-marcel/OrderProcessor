using OrderProcessor.Service.DTO;

namespace OrderProcessor.Web.API.ResponseModels
{
    public class LoginResponse
    {
        public Guid SessionToken { get; set; }
        public CustomerData? CustomerData { get; set; }
    }
}
