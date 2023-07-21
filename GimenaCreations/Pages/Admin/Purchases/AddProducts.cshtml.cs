using GimenaCreations.Constants;
using GimenaCreations.Data;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GimenaCreations.Pages.Admin.Purchases;

public class AddProductsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public AddProductsModel(ApplicationDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    [BindProperty]
    public IList<PurchaseItem> PurchaseItems { get; set; }

    [BindProperty]
    public int PurchaseId { get; set; }

    [BindProperty]
    public IList<CatalogItem> CatalogItems { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.AddProducts)).Succeeded)
        {
            return new ForbidResult();
        }

        PurchaseId = (int)id;
        PurchaseItems = await _context.PurchaseItems.Where(x => x.PurchaseId == id).Include(p => p.CatalogItem).Include(p => p.Purchase).ToListAsync();
        await FillViewInfoAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Purchases.AddProducts)).Succeeded)
        {
            return new ForbidResult();
        }

        if (PurchaseItems == null || !PurchaseItems.Any())
        {
            ModelState.AddModelError(string.Empty, "At least one item must be added.");
            await FillViewInfoAsync();
            return Page();
        }

        _context.PurchaseItems.RemoveRange(await _context.PurchaseItems.Where(x=>x.PurchaseId == PurchaseId).ToListAsync());
        await _context.SaveChangesAsync();
        await _context.PurchaseItems.AddRangeAsync(PurchaseItems);
        await _context.SaveChangesAsync();
        return RedirectToPage("/Admin/Purchases/Index");
    }

    private async Task FillViewInfoAsync()
    {
        CatalogItems = await _context.CatalogItems.AsNoTracking().ToListAsync();
        ViewData["CatalogItems"] = new SelectList(CatalogItems.Select(x => new { ID = x.Id, x.Name }), "ID", "Name");
    }
}
