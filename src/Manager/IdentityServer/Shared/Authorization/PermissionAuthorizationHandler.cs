using Microsoft.AspNetCore.Authorization;

namespace IdentityServer.Shared.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
    {
        var permissionClaim = context.User.FindFirst(
            c => c.Type == CustomClaimTypes.Permissions);

        if (permissionClaim is null)
        {
            return Task.CompletedTask;
        }

        if (!int.TryParse(permissionClaim.Value, out var permissionClaimValue))
        {
            return Task.CompletedTask;
        }

        var userPermission = (Permissions)permissionClaimValue;

        if ((userPermission & requirement.Permission) == 0) return Task.CompletedTask;
        
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}