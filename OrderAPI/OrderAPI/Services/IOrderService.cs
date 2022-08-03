namespace OrderAPI.Services
{
    public interface IOrderService
    {
        Task<string> PublishOrder(string topicName, string message, IConfiguration configuration);
    }
}
