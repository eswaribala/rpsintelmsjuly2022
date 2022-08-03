using PaymentService.Models;
using MongoDB.Driver;
namespace PaymentService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<BsonOrder> _OrdersCollection;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            var mongoClient = new MongoClient(_configuration["ConnectionString"]);

            var database = mongoClient.GetDatabase(_configuration["DatabaseName"]);

            _OrdersCollection = database.GetCollection<BsonOrder>(
             _configuration["OrdersCollectionName"]);

        }
     
        public void AddOrder(BsonOrder Order)
        {
            _OrdersCollection.InsertOneAsync(Order);
        }
    }
}
