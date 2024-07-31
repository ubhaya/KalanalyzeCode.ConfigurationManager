using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly MockAuthUser _mockAuthUser;

    public TestAuthenticationHandler
    (IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        MockAuthUser user)
        : base(options, loggerFactory, encoder)
    {
        _mockAuthUser = user;
    }
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (_mockAuthUser.Claims.Count == 0)
            return Task.FromResult(AuthenticateResult.Fail("Mock auth user not configured."));
        
        // 2. Create the principal and the ticket
        var identity = new ClaimsIdentity(_mockAuthUser.Claims, AuthConstants.Scheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthConstants.Scheme);

        // 3. Authenticate the request
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}

public static class AuthConstants
{
    public const string Scheme = "TestAuth";
}

public static class Extensions
{
    public static AuthenticationBuilder AddTestAuthentication(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(AuthConstants.Scheme)
                .RequireAuthenticatedUser()
                .Build());

        return services.AddAuthentication(AuthConstants.Scheme)
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(AuthConstants.Scheme, options => { });
    }
}