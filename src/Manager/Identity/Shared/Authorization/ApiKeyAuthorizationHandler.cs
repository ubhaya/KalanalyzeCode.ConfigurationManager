using Microsoft.AspNetCore.Authorization;

namespace Identity.Shared.Authorization;

public class ApiKeyAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
    {
        // Todo: Handle Api Key authorization
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}