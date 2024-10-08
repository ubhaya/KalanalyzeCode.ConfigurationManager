using Microsoft.AspNetCore.Authorization;

namespace KalanalyzeCode.ConfigurationManager.Application.Authorization;

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

        var userPermissions = (Permissions)permissionClaimValue;

        if (userPermissions == 0 && requirement.Permission == 0)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if ((userPermissions & requirement.Permission) != 0)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}