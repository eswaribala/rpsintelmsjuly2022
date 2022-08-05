using CQRSCartAPI.Models;
using MongoDB.Driver;

namespace CQRSCartAPI.Repositories
{
    public class CartMongoRepository
    {
        private IConfiguration _configuration;
        private IMongoDatabase _db;
        private string DBName;
        private string CollectionName;
        private string Url;
        public CartMongoRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            DBName = _configuration["MongoDbName"];
            CollectionName = _configuration["MongoCollectionName"];
            Url = _configuration["Url"];
            MongoClient _client = new MongoClient(Url);
            _db = _client.GetDatabase(DBName);
        }
       
        public List<CartEntity> GetCarts()
        {
            return _db.GetCollection<CartEntity>(CollectionName).Find(_ => true).ToList();
        }
        public CartEntity GetCart(long id)
        {
            return _db.GetCollection<CartEntity>(CollectionName).Find
                (cart => cart.CartId == id).SingleOrDefault();
        }
        public CartEntity GetCartByCartName(string CartName)
        {
            return _db.GetCollection<CartEntity>(CollectionName).Find
                (cart => cart.CartName == CartName).Single();
        }
        public void Create(CartEntity CartEntity)
        {
            _db.GetCollection<CartEntity>(CollectionName).InsertOne(CartEntity);
        }
        public void Update(CartEntity Cart)
        {
            var filter = Builders<CartEntity>.Filter.Where(_ => _.CartId == 
            Cart.CartId);
            _db.GetCollection<CartEntity>(CollectionName).ReplaceOne(filter, Cart);
        }
        public void Remove(long id)
        {
            var filter = Builders<CartEntity>.Filter.Where(_ => _.CartId == id);
            var operation = _db.GetCollection<CartEntity>(CollectionName).
                DeleteOne(filter);
        }


    }
}
