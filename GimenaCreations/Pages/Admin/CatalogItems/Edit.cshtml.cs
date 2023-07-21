using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Helpers;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.CatalogItems
{
    public class EditModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IFileHelper _fileHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(Data.ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IFileHelper fileHelper, IAuthorizationService authorizationService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _fileHelper = fileHelper;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public CatalogItem CatalogItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogItems.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.CatalogItems == null)
            {
                return NotFound();
            }

            var catalogitem = await _context.CatalogItems.FirstOrDefaultAsync(m => m.Id == id);
            if (catalogitem == null)
            {
                return NotFound();
            }
            CatalogItem = catalogitem;
            ViewData["CatalogTypeId"] = new SelectList(_context.CatalogTypes, "Id", "Type");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogItems.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (CatalogItem.FormFile != null && CatalogItem.FormFile.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(CatalogItem.PictureFileName))
                    {
                        System.IO.File.Delete($"{_webHostEnvironment.WebRootPath}\\{CatalogItem.PictureFileName}");
                    }
                    
                    await _fileHelper.CreateFileAsync($"{_webHostEnvironment.WebRootPath}\\{CatalogItem.FormFile.FileName}", CatalogItem.FormFile);                    
                    CatalogItem.PictureFileName = CatalogItem.FormFile.FileName;
                }

                _context.Attach(CatalogItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatalogItemExists(CatalogItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CatalogItemExists(int id)
        {
            return _context.CatalogItems.Any(e => e.Id == id);
        }
    }
}
