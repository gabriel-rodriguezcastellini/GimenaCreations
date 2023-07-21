using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using GimenaCreations.Extensions;
using Microsoft.AspNetCore.Authorization;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(Data.ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Orders.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(x=>x.Items).ThenInclude(x=>x.Files).FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            Order = order;
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["Status"] = new SelectList(Enum.GetValues<OrderStatus>().Select(x => new { ID = (int)x, Name = x.GetDisplayName() }), "ID", "Name", (int)order.Status);
            ViewData["PaymentMethod"] = new SelectList(Enum.GetValues<PaymentMethod>().Select(x => new { ID = (int)x, Name = x.GetDisplayName() }), "ID", "Name", (int)order.PaymentMethod);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Orders.Edit)).Succeeded)
            {
                return new ForbidResult();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.Id))
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

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
