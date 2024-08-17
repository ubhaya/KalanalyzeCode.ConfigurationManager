using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using KalanalyzeCode.ConfigurationManager.Ui.Client;

namespace KalanalyzeCode.ConfigurationManager.Ui;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
            new(AppConstants.Identity.ScopeName, AppConstants.Identity.DisplayName)
        };

    public static IEnumerable<Duende.IdentityServer.Models.Client> Clients =>
        new Duende.IdentityServer.Models.Client[]
        {
            new()
            {
                ClientId = "postman",
                ClientName = "PostMan",
                AllowedGrantTypes = GrantTypes.Code,
                
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    AppConstants.Identity.ScopeName
                ],
                AllowOfflineAccess = true,
                    
                RedirectUris = {"https://localhost:7206/swagger/oauth2-redirect.html"},
                AllowedCorsOrigins = {"https://localhost:7206"},
                
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding
            },
            new()
            {
                ClientId = "postman.apikey",
                ClientName = "PostMan.api",
                AllowedGrantTypes = ["api_key"],
                
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    AppConstants.Identity.ScopeName
                ],
                AllowOfflineAccess = true,
                
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding
            },
            new()
            {
                ClientId = AppConstants.Identity.ClientId ,
                ClientName = AppConstants.Identity.ClientName  ,
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                    
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                    
                AllowedScopes = [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    AppConstants.Identity.ScopeName
                ],
                AllowOfflineAccess = true,
                    
                AllowedCorsOrigins = {"https://localhost:7206"},
                RedirectUris = {"https://localhost:7206/signin-oidc"},
                PostLogoutRedirectUris = {"https://localhost:7206/signout-callback-oidc"},
                //
                // RefreshTokenUsage = TokenUsage.OneTimeOnly,
                // RefreshTokenExpiration = TokenExpiration.Sliding
            },
        };
}