using CatalogAPI.Models;

namespace CatalogAPI.Repositories
{
    public interface ICatalogRepo
    {
        Task<Catalog> AddCatalog(Catalog Catalog);
        Task<bool> DeleteCatalog(long CatalogId);

        Task<Catalog> GetCatalogById(long CatalogId);
        Task<IEnumerable<Catalog>> GetCatalogs();

        Task<Catalog> UpdateCatalog(Catalog Catalog);
    }
}
