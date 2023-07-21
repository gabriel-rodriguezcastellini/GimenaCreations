using GimenaCreations.Data;
using GimenaCreations.Entities;
using GimenaCreations.Pagination;
using Microsoft.EntityFrameworkCore;

namespace GimenaCreations.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CatalogService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<CatalogItem> GetCatalogItemAsync(int? id)
        {
            if (_context.CatalogItems == null)
            {
                return null;
            }

            var catalogitem = await _context.CatalogItems.Where(x => x.IsVisible).Include(x => x.CatalogType).FirstOrDefaultAsync(m => m.Id == id);

            if (catalogitem == null)
            {
                return null;
            }

            return catalogitem;
        }

        public async Task<PaginatedList<CatalogItem>> GetCatalogItemsAsync(string searchString, int? catalogTypeId, int? pageIndex)
        {
            PaginatedList<CatalogItem> items = default!;

            if (_context.CatalogItems != null)
            {
                IQueryable<CatalogItem> catalogItems = _context.CatalogItems.Where(x => x.IsVisible).AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    catalogItems = catalogItems.Where(s => s.Name.Contains(searchString));
                }

                if (catalogTypeId.HasValue)
                {
                    catalogItems = catalogItems.Where(s => s.CatalogTypeId == catalogTypeId);
                }

                var pageSize = _configuration.GetValue("PageSize", 4);
                return await PaginatedList<CatalogItem>.CreateAsync(catalogItems.AsNoTracking().Include(c => c.CatalogType), pageIndex ?? 1, pageSize);
            }

            return items;
        }

        public async Task<IList<CatalogType>> GetCatalogTypesAsync() => await _context.CatalogTypes.ToListAsync();

        public async Task<List<CatalogItem>> GetItemsWithCriticalStockAsync()
        {
            return await _context.CatalogItems.Where(x => x.AvailableStock < x.RestockThreshold && !x.OnReorder).ToListAsync();
        }

        public async Task UpdateCatalogItemStockAsync(int catalogItemId, int substractedQuantity)
        {
            var catalogItem = await _context.CatalogItems.FirstAsync(x => x.Id == catalogItemId);
            catalogItem.AvailableStock += substractedQuantity;
            _context.Update(catalogItem);
            await _context.SaveChangesAsync();
        }
    }
}
