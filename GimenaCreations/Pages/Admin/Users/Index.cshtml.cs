using GimenaCreations.Constants;
using GimenaCreations.Data;
using GimenaCreations.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GimenaCreations.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public IList<ApplicationUser> Users { get; set; } = default!;
        
        public async Task<IActionResult> OnGetAsync()
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Users.View)).Succeeded)
            {
                return new ForbidResult();
            }

            if (_context.Users != null)
            {
                Users = await _context.Users.Where(x => x.Id != _userManager.GetUserId(HttpContext.User)).ToListAsync();
            }

            foreach (var item in Users)
            {
                foreach (var role in _roleManager.Roles.Select(x => x.Name))
                {
                    if (await _userManager.IsInRoleAsync(item, role))
                    {
                        item.Roles.Add(role);
                    }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(string id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Users.ChangeStatus)).Succeeded)
            {
                return new ForbidResult();
            }

            var user = await _context.Users.FirstAsync(x => x.Id == id);
            user.Active = !user.Active;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
