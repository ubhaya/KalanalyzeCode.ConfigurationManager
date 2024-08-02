using System.Diagnostics;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints;

[Collection(Collections.ApiWebApplicationCollection)]
public class GetAppSettingEndpointTests : TestBase
{
    private readonly IAppSettingsClient _client;

    public GetAppSettingEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _client = new AppSettingsClient(factory.HttpClient);
    }

    
    
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenValidSettingNamesParse()
    {
        // Arrange
        var testSettings = "TestSettings";
        await AddAsync(new ConfigurationSettings() { Id = testSettings, Value = "true" });

        // Act
        var settings = await _client.GetAsync(testSettings, CancellationToken);
        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        Debug.Assert(settings.Settings is not null);
        settings.Settings.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenInvalidSettingNamesParse()
    {
        // Arrange

        // Act
        var settings = await _client.GetAsync("invalidName", CancellationToken);

        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        Debug.Assert(settings.Settings is not null);
        settings.Settings.Should().BeNullOrEmpty();
    }
}