using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using GimenaCreations.Helpers;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Suppliers
{
    public class EditModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IFileHelper _fileHelper;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(Data.ApplicationDbContext context, IFileHelper fileHelper, IAuthorizationService authorizationService)
        {
            _context = context;
            _fileHelper = fileHelper;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Supplier Supplier { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier =  await _context.Suppliers.FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            Supplier = supplier;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }            

            try
            {
                if (Supplier.FormFile == null)
                {
                    Supplier.Image = null;
                }
                else
                {
                    var file = await _fileHelper.GetFileAsync(Supplier.FormFile);

                    if (file.Item2 != null)
                    {
                        ModelState.AddModelError(string.Empty, file.Item2);
                        return Page();
                    }

                    Supplier.Image = file.Item1;
                }

                _context.Attach(Supplier).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(Supplier.Id))
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

        private bool SupplierExists(int id)
        {
          return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
