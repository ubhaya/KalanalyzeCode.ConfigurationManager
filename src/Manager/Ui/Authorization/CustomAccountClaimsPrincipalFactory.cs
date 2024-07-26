using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace MdUi.Authorization;

public class CustomAccountClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    public CustomAccountClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
    {
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);
        
        var identity = (ClaimsIdentity)user.Identity!;

        if (account != null)
        {
            foreach (var (key, value) in account.AdditionalProperties)
            {
                if (value != null &&
                    value is JsonElement element && element.ValueKind == JsonValueKind.Array)
                {
                    identity.RemoveClaim(identity.FindFirst(key));

                    var claims = element.EnumerateArray()
                        .Select(x => new Claim(key, x.ToString()));

                    identity.AddClaims(claims);
                }
            }
        }

        return user;
    }
}