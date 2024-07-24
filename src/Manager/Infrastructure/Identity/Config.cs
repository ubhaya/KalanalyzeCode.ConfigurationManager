using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Infrastructure.Identity;

public static class Config
{
    private static readonly List<string> AllowedScopes =
    [
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile
    ];
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new(AppConstants.Identity.ScopeName, AppConstants.Identity.DisplayName),
        };

    public static IEnumerable<Client> Clients()
    {
        var allowedScopes = AppConstants.Identity.AllowedScope.Concat(AllowedScopes).ToList();
        return new Client[]
        {
            new()
            {
                ClientId = AppConstants.Identity.ClientId,
                ClientName = AppConstants.Identity.ClientName,
                AllowedGrantTypes = GrantTypes.Code,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = allowedScopes,
                AllowOfflineAccess = true,

                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding
            },
            new()
            {
                ClientId = "CleanArchitecture.Maui.MobileUi.Postman",
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "clean_architecture_maui_api",
                ],
            },
        };
    }
    
    public static List<TestUser> Users =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "alice",
                Password = "password"
            }
        };
}