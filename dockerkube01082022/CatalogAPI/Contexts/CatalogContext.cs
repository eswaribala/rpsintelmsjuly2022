using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Contexts
{
    public class CatalogContext:DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options)
           : base(options)
        {

            this.Database.EnsureCreated();
        }

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
