using GimenaCreations.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Claims;

namespace GimenaCreations.Helpers;

public static class ClaimsHelper
{
    public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy)
    {        
        foreach (FieldInfo fi in policy.GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            allPermissions.Add(new RoleClaimsViewModel { Value = fi.GetValue(null).ToString(), Type = "Permissions" });
        }
    }

    public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
    {        
        if (!(await roleManager.GetClaimsAsync(role)).Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
    }
}
