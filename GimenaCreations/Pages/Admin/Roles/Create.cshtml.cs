using GimenaCreations.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GimenaCreations.Pages.Admin.Roles;

public class CreateModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthorizationService _authorizationService;

    public CreateModel(RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
    {
        _roleManager = roleManager;
        _authorizationService = authorizationService;
    }

    [BindProperty]
    public IdentityRole Role { get; set; }

    public async Task<IActionResult> OnGet()
    {
        if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Roles.Create)).Succeeded)
        {
            return new ForbidResult();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Roles.Create)).Succeeded)
        {
            return new ForbidResult();
        }

        if (ModelState.IsValid)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(Role.Name));

            if (result.Succeeded)
            {
                return RedirectToPage("Index");
            }
            else
            {
                Errors(result);
            }
        }

        return RedirectToPage("Create");
    }

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    }
}
