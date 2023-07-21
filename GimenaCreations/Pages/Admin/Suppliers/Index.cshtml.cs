using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using GimenaCreations.PDF.Presentation;
using GimenaCreations.PDF.DocumentModels;
using QuestPDF.Fluent;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Suppliers
{
    public class IndexModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public IndexModel(Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public IList<Supplier> Supplier { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (_context.Suppliers != null)
            {
                Supplier = await _context.Suppliers.ToListAsync();
            }

            return Page();
        }

        public async Task<FileContentResult> OnGetImageAsync(int id)
        {
            byte[] byteArray = (await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id)).Image;
            return byteArray != null ? new FileContentResult(byteArray, "image/jpeg") : null;
        }

        public async Task<IActionResult> OnGetReportAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Suppliers.View)).Succeeded)
            {
                return new ForbidResult();
            }

            var document = new SupplierDocument((await _context.Suppliers.AsNoTracking().ToListAsync()).Select(x => new SupplierModel
            {
                BusinessName = x.BusinessName,
                ContactName = x.ContactName,
                ContactPhone = x.ContactPhone,
                Id = x.Id,
                Type = x.SupplierType
            }).ToList());

            document.GeneratePdf("suppliers.pdf");
            Process.Start("explorer.exe", "suppliers.pdf");
            return RedirectToPage("Index");
        }
    }
}
