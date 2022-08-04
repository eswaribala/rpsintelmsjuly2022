namespace CamundaPaymentAPI.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public string? CustomerId { get; set; }
        public string? OrderDate { get; set; }
        public long? OrderAmount { get; set; }
        
    }
}
