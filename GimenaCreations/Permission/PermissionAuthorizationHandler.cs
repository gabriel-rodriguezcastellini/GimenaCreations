using Microsoft.AspNetCore.Authorization;

namespace GimenaCreations.Permission;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    public PermissionAuthorizationHandler()
    {

    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User == null)
        {
            return Task.CompletedTask;
        }        

        if (context.User.Claims.Where(x => x.Type == "Permission" && x.Value == requirement.Permission && x.Issuer == "LOCAL AUTHORITY").Any())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
