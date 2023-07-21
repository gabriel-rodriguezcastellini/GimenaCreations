using GimenaCreations.Constants;
using GimenaCreations.Data;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GimenaCreations.Pages.Admin.PurchaseReceptions;

public class AddItemsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public AddItemsModel(ApplicationDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    [BindProperty]
    public int PurchaseReceptionId { get; set; }

    [BindProperty]
    public IList<PurchaseReceptionItem> Items { get; set; }

    [BindProperty]
    public IList<CatalogItem> CatalogItems { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (!(await _authorizationService.AuthorizeAsync(User, Permissions.PurchaseReceptions.AddItems)).Succeeded)
        {
            return new ForbidResult();
        }

        PurchaseReceptionId = (int)id;
        Items = await _context.PurchaseReceptionItems.Where(x => x.PurchaseReceptionId == id).Include(x => x.CatalogItem).Include(x => x.PurchaseReception).ToListAsync();
        await FillViewInfoAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!(await _authorizationService.AuthorizeAsync(User, Permissions.PurchaseReceptions.AddItems)).Succeeded)
        {
            return new ForbidResult();
        }

        if (Items == null || !Items.Any())
        {
            ModelState.AddModelError(string.Empty, "At least one item must be added.");
            await FillViewInfoAsync();
            return Page();
        }

        foreach (var item in await _context.PurchaseReceptionItems.Where(x => x.IsSatisfied && x.PurchaseReceptionId == Items.First().PurchaseReceptionId).ToListAsync())
        {
            var catalogItem = await _context.CatalogItems.FirstAsync(x => x.Id == item.CatalogItemId);
            catalogItem.AvailableStock -= item.Quantity;
            _context.Update(catalogItem);
            await _context.SaveChangesAsync();
        }

        _context.PurchaseReceptionItems.RemoveRange(await _context.PurchaseReceptionItems.Where(x => x.PurchaseReceptionId == PurchaseReceptionId).ToListAsync());
        await _context.SaveChangesAsync();

        foreach (var item in Items)
        {
            item.ProductName = (await _context.CatalogItems.AsNoTracking().FirstAsync(x => x.Id == item.CatalogItemId)).Name;
            item.ProductType = (await _context.CatalogItems.Include(x => x.CatalogType).AsNoTracking().FirstAsync(x => x.Id == item.CatalogItemId)).CatalogType.Type;
        }

        await _context.PurchaseReceptionItems.AddRangeAsync(Items);
        await _context.SaveChangesAsync();

        foreach (var item in Items)
        {
            if (item.IsSatisfied)
            {
                var catalogItem = await _context.CatalogItems.FirstAsync(x => x.Id == item.CatalogItemId);
                catalogItem.AvailableStock += item.Quantity;
                _context.Update(catalogItem);
                await _context.SaveChangesAsync();
            }
        }

        var purchaseReception = await _context.PurchaseReceptions.AsNoTracking().FirstAsync(x => x.Id == PurchaseReceptionId);
        var purchase = await _context.Purchases.Include(x => x.Items).FirstAsync(x => x.Id == purchaseReception.PurchaseId);
        var purchaseReceptions = await _context.PurchaseReceptions.Include(x => x.PurchaseReceptionItems).Where(x => x.PurchaseId == purchase.Id).ToListAsync();

        purchase.PurchaseStatus = purchaseReceptions.All(x => x.PurchaseReceptionItems.All(x => x.IsSatisfied)) && VerifyReceptionItems(purchase.Items, purchaseReceptions)
            ? PurchaseStatus.Delivered :
            PurchaseStatus.PartiallyDelivered;

        _context.Update(purchase);
        await _context.SaveChangesAsync();
        return RedirectToPage("/Admin/PurchaseReceptions/Index");
    }

    private async Task FillViewInfoAsync()
    {
        CatalogItems = await _context.CatalogItems.AsNoTracking().ToListAsync();
        ViewData["CatalogItems"] = new SelectList(CatalogItems.Select(x => new { ID = x.Id, x.Name }), "ID", "Name");
    }

    private static bool VerifyReceptionItems(List<PurchaseItem> purchaseItems, List<PurchaseReception> purchaseReceptions)
    {
        var groupedPurchaseItems = new List<PurchaseItem>();
        var groupedPurchaseReceptionItems = new List<PurchaseReceptionItem>();
        purchaseItems.GroupBy(x => x.CatalogItemId).ToList().ForEach(x => groupedPurchaseItems.Add(new PurchaseItem { CatalogItemId = x.First().CatalogItemId, Quantity = x.Sum(x => x.Quantity) }));

        foreach (var item in purchaseReceptions)
        {
            foreach (var receptionItem in item.PurchaseReceptionItems)
            {
                if (groupedPurchaseReceptionItems.Any(x => x.CatalogItemId == receptionItem.CatalogItemId))
                {
                    groupedPurchaseReceptionItems.First(x => x.CatalogItemId == receptionItem.CatalogItemId).Quantity += receptionItem.Quantity;
                }
                else
                {
                    groupedPurchaseReceptionItems.Add(new PurchaseReceptionItem { CatalogItemId = receptionItem.CatalogItemId, Quantity = receptionItem.Quantity });
                }
            }
        }

        bool missingItems = groupedPurchaseItems.Select(x => x.CatalogItemId).Intersect(groupedPurchaseReceptionItems.Select(x => x.CatalogItemId)).Count() < groupedPurchaseItems.Count;

        if (missingItems)
        {
            return false;
        }

        foreach (var item in groupedPurchaseItems)
        {
            if (!groupedPurchaseReceptionItems.Any(x => x.CatalogItemId == item.CatalogItemId && x.Quantity >= item.Quantity))
            {
                return false;
            }
        }

        return true;
    }
}
