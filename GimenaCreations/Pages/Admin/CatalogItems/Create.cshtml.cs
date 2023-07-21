using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GimenaCreations.Helpers;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.CatalogItems
{
    public class CreateModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IFileHelper _fileHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthorizationService _authorizationService;

        public CreateModel(Data.ApplicationDbContext context, IFileHelper fileHelper, IWebHostEnvironment webHostEnvironment, IAuthorizationService authorizationService)
        {
            _context = context;
            _fileHelper = fileHelper;
            _webHostEnvironment = webHostEnvironment;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogItems.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            ViewData["CatalogTypeId"] = new SelectList(_context.CatalogTypes, "Id", "Type");
            return Page();
        }

        [BindProperty]
        public CatalogItem CatalogItem { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogItems.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (CatalogItem.FormFile != null && CatalogItem.FormFile.Length > 0)
            {
                await _fileHelper.CreateFileAsync($"{_webHostEnvironment.WebRootPath}\\{CatalogItem.FormFile.FileName}", CatalogItem.FormFile);
                CatalogItem.PictureFileName = CatalogItem.FormFile.FileName;
            }

            _context.CatalogItems.Add(CatalogItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
