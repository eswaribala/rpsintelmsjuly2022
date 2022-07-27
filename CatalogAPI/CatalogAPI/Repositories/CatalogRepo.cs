using CatalogAPI.Contexts;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repositories
{
    public class CatalogRepo : ICatalogRepo
    {
        private readonly CatalogContext _db;

        public CatalogRepo(CatalogContext catalogContext)
        {
            _db = catalogContext;
        }

        public async Task<Catalog> AddCatalog(Catalog Catalog)
        {

            var result = await this._db.Catalogs.AddAsync(Catalog);

            await this._db.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> DeleteCatalog(long CatalogId)
        {
            var result = await this._db.Catalogs.FirstOrDefaultAsync(c => c.CatalogId
          == CatalogId);
            if (result != null)
            {
                this._db.Catalogs.Remove(result);
                await this._db.SaveChangesAsync();
            }
            result = await this._db.Catalogs.FirstOrDefaultAsync(c => c.CatalogId
              == CatalogId);
            if (result == null)
                return true;
            else
                return false;
        }

        public async Task<Catalog> GetCatalogById(long CatalogId)
        {
            var result = await this._db.Catalogs.FirstOrDefaultAsync(c => c.CatalogId
          == CatalogId);
            if (result != null)
                return result;
            else
                return null;
        }

        public async Task<IEnumerable<Catalog>> GetCatalogs()
        {
            return await this._db.Catalogs.ToListAsync();
        }

        public async Task<Catalog> UpdateCatalog(Catalog Catalog)
        {
            var result = await this._db.Catalogs.FirstOrDefaultAsync(c => c.CatalogId
            == Catalog.CatalogId);

            if (result != null)
            {
                result.CatalogName = Catalog.CatalogName;
                await this._db.SaveChangesAsync();
                return result;
            }
            else
                return null;
        }
    }
}
