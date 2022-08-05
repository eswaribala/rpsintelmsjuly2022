using CQRSCartAPI.Contexts;
using CQRSCartAPI.Models;

namespace CQRSCartAPI.Repositories
{
    public class CartSqliteRepository
    {
        private readonly CartContext _CartContext;

        public CartSqliteRepository(CartContext cartContext)
        {
            _CartContext = cartContext; 
        }
        public Cart Create(Cart Cart)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Cart> entry =
                _CartContext.Carts.Add(Cart);
            _CartContext.SaveChanges();
            return entry.Entity;
        }
        public void Update(Cart Cart)
        {
            _CartContext.Carts.Update(Cart);
            _CartContext.SaveChanges();
        }
        public void Remove(long id)
        {
            _CartContext.Carts.Remove(GetById(id));
            _CartContext.SaveChanges();
        }
        public IQueryable<Cart> GetAll()
        {
            return _CartContext.Carts;
        }
        public Cart GetById(long id)
        {
            return _CartContext.Carts.Find(id);
        }

    }
}
