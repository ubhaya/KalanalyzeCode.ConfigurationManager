using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Api.IntegrationTests;

public class ApiWebApplication : WebApplicationFactory<KalanalyzeCode.ConfigurationManager.Api.Api>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Todo: configure test services here 
        });

        return base.CreateHost(builder);
    }
}