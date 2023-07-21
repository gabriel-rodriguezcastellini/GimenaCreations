using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GimenaCreations.Constants;

namespace GimenaCreations.Pages.Admin.LoginLogoutReport
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

        public IList<LoginLogoutAudit> LoginLogoutAudit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.LoginLogoutReport.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (_context.LoginLogoutAudits != null)
            {
                LoginLogoutAudit = await _context.LoginLogoutAudits.OrderByDescending(x => x.Id).ToListAsync();
            }

            return Page();
        }
    }
}
