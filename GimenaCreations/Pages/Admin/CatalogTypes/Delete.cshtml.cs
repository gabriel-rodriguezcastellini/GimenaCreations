﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.CatalogTypes
{
    public class DeleteModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public DeleteModel(Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public CatalogType CatalogType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogTypes.Delete)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.CatalogTypes == null)
            {
                return NotFound();
            }

            var catalogtype = await _context.CatalogTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (catalogtype == null)
            {
                return NotFound();
            }
            else
            {
                CatalogType = catalogtype;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.CatalogTypes.Delete)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.CatalogTypes == null)
            {
                return NotFound();
            }
            var catalogtype = await _context.CatalogTypes.FindAsync(id);

            if (catalogtype != null)
            {
                CatalogType = catalogtype;
                _context.CatalogTypes.Remove(CatalogType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
