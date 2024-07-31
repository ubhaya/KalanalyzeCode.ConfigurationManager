using System.Diagnostics;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints;

[Collection("Test collection")]
public class GetAppSettingEndpointTests : TestBase, IAsyncLifetime
{
    private readonly IAppSettingsClient _client;
    private readonly IServiceScope _scope;
    private readonly Func<Task> _resetDatabase;

    public GetAppSettingEndpointTests(ApiWebApplication factory)
    {
        _client = new AppSettingsClient(factory.HttpClient);
        _scope = factory.Scope;
        _resetDatabase = factory.ResetDatabaseAsync;
    }

    
    
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenValidSettingNamesParse()
    {
        // Arrange
        var testSettings = "TestSettings";
        await AddAsync(_scope, new ConfigurationSettings() { Id = testSettings, Value = "true" });

        // Act
        var settings = await _client.GetAppSettingsRequestAsync(testSettings);

        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        settings.Success.Should().BeTrue();
        settings.Data.Should().NotBeNull();
        Debug.Assert(settings.Data is not null);
        settings.Data.Settings.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenInvalidSettingNamesParse()
    {
        // Arrange

        // Act
        var settings = await _client.GetAppSettingsRequestAsync("invalidName");

        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        settings.Success.Should().BeFalse();
        settings.Data.Should().NotBeNull();
        Debug.Assert(settings.Data is not null);
        settings.Data.Settings.Should().BeNullOrEmpty();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}