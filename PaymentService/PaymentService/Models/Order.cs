namespace PaymentService.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public long OrderAmount { get; set; }
        public string? Description { get; set; }
    }
}
