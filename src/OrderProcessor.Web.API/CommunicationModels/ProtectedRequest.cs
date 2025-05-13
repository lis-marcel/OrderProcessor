namespace OrderProcessor.Web.API.CommunicationModels
{
    public class ProtectedRequest
    {
        public Guid SessionToken { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
