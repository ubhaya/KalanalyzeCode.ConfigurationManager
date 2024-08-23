using System.Security.Claims;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using Microsoft.AspNetCore.Authorization;

namespace KalanalyzeCode.ConfigurationManager.Application.Authorization;

public sealed class ProjectAuthorizationHandler :
    AuthorizationHandler<ProjectApiKeyRequirement, Project>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectApiKeyRequirement requirement,
        Project resource)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (context.User is null || (!context.User.Identity?.IsAuthenticated ?? false))
        {
            context.Fail(new AuthorizationFailureReason(this, "User is not authenticated"));
            return Task.CompletedTask;
        }
        
        var apiKeyClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        
        if (apiKeyClaim is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "Invalid Api Key for the project."));
            return Task.CompletedTask;
        }

        if (!Guid.TryParse(apiKeyClaim.Value, out var apiKey))
        {
            context.Fail(new AuthorizationFailureReason(this, $"Invalid Api {apiKeyClaim.Value}"));
            return Task.CompletedTask;
        }

        if (apiKey == resource.ApiKey)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}