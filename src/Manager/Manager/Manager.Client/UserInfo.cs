using System.Security.Claims;
using IdentityModel;
using KalanalyzeCode.ConfigurationManager.Ui.Client.Authorization;

namespace KalanalyzeCode.ConfigurationManager.Ui.Client;

// Add properties to this class and update the server and client AuthenticationStateProviders
// to expose more information about the authenticated user to the client.
public sealed class UserInfo
{
    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Permissions { get; init; }
    public required IEnumerable<string> Roles { get; init; }

    private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
        principal.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");
}