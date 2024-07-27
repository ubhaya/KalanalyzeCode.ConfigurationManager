using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer
{
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

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = [
                            AppConstants.Identity.ScopeName
                        ]
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientName = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "scope2" }
                },
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
                    
                    RedirectUris = {"https://localhost:7015/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = {"https://localhost:7015"},

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
                    
                    AllowedCorsOrigins = {"https://localhost:7016"},
                    RedirectUris = {"http://localhost:5163/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5163/signout-callback-oidc"},
                    //
                    // RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    // RefreshTokenExpiration = TokenExpiration.Sliding
                },
            };
    }
}
