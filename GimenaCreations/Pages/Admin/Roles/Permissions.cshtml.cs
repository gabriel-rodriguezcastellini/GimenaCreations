using GimenaCreations.Constants;
using GimenaCreations.Helpers;
using GimenaCreations.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace GimenaCreations.Pages.Admin.Roles;

public class PermissionsModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthorizationService _authorizationService;

    public PermissionsModel(RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
    {
        _roleManager = roleManager;
        _authorizationService = authorizationService;
    }

    [BindProperty]
    public PermissionViewModel Model { get; set; }

    public async Task<IActionResult> OnGetAsync(string roleId)
    {
        if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Roles.Permissions)).Succeeded)
        {
            return new ForbidResult();
        }

        var model = new PermissionViewModel();
        var allPermissions = new List<RoleClaimsViewModel>();
        allPermissions.GetPermissions(typeof(Permissions.Admin));
        allPermissions.GetPermissions(typeof(Permissions.AuditEntries));
        allPermissions.GetPermissions(typeof(Permissions.CatalogItems));
        allPermissions.GetPermissions(typeof(Permissions.CatalogTypes));
        allPermissions.GetPermissions(typeof(Permissions.LoginLogoutReport));
        allPermissions.GetPermissions(typeof(Permissions.OrderItems));
        allPermissions.GetPermissions(typeof(Permissions.Orders));        
        allPermissions.GetPermissions(typeof(Permissions.PurchaseReceptions));
        allPermissions.GetPermissions(typeof(Permissions.Purchases));
        allPermissions.GetPermissions(typeof(Permissions.PurchasesChangeHistory));
        allPermissions.GetPermissions(typeof(Permissions.Roles));
        allPermissions.GetPermissions(typeof(Permissions.Suppliers));
        allPermissions.GetPermissions(typeof(Permissions.Users));
        allPermissions.GetPermissions(typeof(Permissions.CriticalStock));
        allPermissions.GetPermissions(typeof(Permissions.HealthCheck));
        var role = await _roleManager.FindByIdAsync(roleId);
        model.RoleId = roleId;
        var claims = await _roleManager.GetClaimsAsync(role);
        var allClaimValues = allPermissions.Select(a => a.Value).ToList();
        var roleClaimValues = claims.Select(a => a.Value).ToList();
        var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

        foreach (var permission in allPermissions)
        {
            if (authorizedClaims.Any(a => a == permission.Value))
            {
                permission.Selected = true;
            }
        }

        model.RoleClaims = allPermissions;
        Model = model;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!(await _authorizationService.AuthorizeAsync(User, Permissions.Roles.Permissions)).Succeeded)
        {
            return new ForbidResult();
        }

        var role = await _roleManager.FindByIdAsync(Model.RoleId);

        foreach (var claim in await _roleManager.GetClaimsAsync(role))
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        foreach (var claim in Model.RoleClaims.Where(a => a.Selected).ToList())
        {
            await _roleManager.AddPermissionClaim(role, claim.Value);
        }

        return RedirectToPage("Permissions", new { roleId = Model.RoleId });
    }
}
