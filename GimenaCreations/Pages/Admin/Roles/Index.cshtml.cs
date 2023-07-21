using GimenaCreations.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimenaCreations.Pages.Admin.Roles;

public class IndexModel : PageModel
{
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IAuthorizationService authorizationService;

    public IndexModel(RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
    {
        this.roleManager = roleManager;
        this.authorizationService = authorizationService;
    }

    public IQueryable<IdentityRole> Roles { get; set; }

    public async Task<IActionResult> OnGet()
    {
        if (!(await authorizationService.AuthorizeAsync(User, Permissions.Roles.View)).Succeeded)
        {
            return new ForbidResult();
        }

        Roles = roleManager.Roles;
        return Page();
    }

    [BindProperty]
    public string RoleToDelete { get; set; }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        if (!(await authorizationService.AuthorizeAsync(User, Permissions.Roles.Delete)).Succeeded)
        {
            return new ForbidResult();
        }

        IdentityRole role = await roleManager.FindByIdAsync(RoleToDelete);

        if (role != null)
        {
            IdentityResult result = await roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToPage("Index");
            }
            else
            {
                Errors(result);
            }
        }
        else
        {
            ModelState.AddModelError("", "No role found");
        }

        Roles = roleManager.Roles;
        return Page();
    }

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    }
}
