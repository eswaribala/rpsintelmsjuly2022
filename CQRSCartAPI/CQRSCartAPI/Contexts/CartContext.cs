using CQRSCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CQRSCartAPI.Contexts
{
    public class CartContext:DbContext
    {
        public CartContext(DbContextOptions<CartContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
            this.Database.Migrate();
        }
       
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
