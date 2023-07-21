using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimenaCreations.Entities;
using GimenaCreations.Helpers;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Suppliers
{
    public class CreateModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IFileHelper _fileHelper;
        private readonly IAuthorizationService _authorizationService;

        public CreateModel(Data.ApplicationDbContext context, IFileHelper fileHelper, IAuthorizationService authorizationService)
        {
            _context = context;
            _fileHelper = fileHelper;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            return Page();
        }

        [BindProperty]
        public Supplier Supplier { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.Create)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Supplier.FormFile != null)
            {
                var file = await _fileHelper.GetFileAsync(Supplier.FormFile);

                if (file.Item2 != null)
                {
                    ModelState.AddModelError(string.Empty, file.Item2);
                    return Page();
                }

                Supplier.Image = file.Item1;
            }

            Supplier.AddDate = DateTime.UtcNow;
            _context.Suppliers.Add(Supplier);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
