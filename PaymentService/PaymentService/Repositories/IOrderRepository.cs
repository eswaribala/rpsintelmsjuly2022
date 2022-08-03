using PaymentService.Models;

namespace PaymentService.Repositories
{
    public interface IOrderRepository
    {
        void AddOrder(BsonOrder Order);
    }
}
