using OrderProcessor.BO.OrderOptions;

namespace OrderProcessor.Web.API.BO
{
    public class OrderRequest
    {
        public double Value { get; set; }
        public required string ProductName { get; set; }
        public required string ShippingAddress { get; set; }
        public int Quantity { get; set; }
        public int CustomerId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
