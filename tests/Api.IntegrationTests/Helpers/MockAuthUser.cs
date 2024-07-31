using System.Security.Claims;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

public class MockAuthUser
{
    public List<Claim> Claims { get; private set; }

    public MockAuthUser(IEnumerable<Claim> claims)
        => Claims = claims.ToList();
}