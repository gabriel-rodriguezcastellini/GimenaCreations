using GimenaCreations.Entities;
using GimenaCreations.Pagination;

namespace GimenaCreations.Services
{
    public interface ICatalogService
    {
        Task<PaginatedList<CatalogItem>> GetCatalogItemsAsync(string searchString, int? catalogTypeId, int? pageIndex);
        Task<IList<CatalogType>> GetCatalogTypesAsync();
        Task<CatalogItem> GetCatalogItemAsync(int? id);
        Task UpdateCatalogItemStockAsync(int catalogItemId, int substractedQuantity);
        Task<List<CatalogItem>> GetItemsWithCriticalStockAsync();
    }
}
