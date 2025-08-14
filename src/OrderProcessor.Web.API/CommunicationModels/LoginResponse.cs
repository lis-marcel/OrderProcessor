using OrderProcessor.Service.DTO;

namespace OrderProcessor.Web.API.CommunicationModels
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public UserAccountDto? CustomerDto { get; set; }
    }
}
