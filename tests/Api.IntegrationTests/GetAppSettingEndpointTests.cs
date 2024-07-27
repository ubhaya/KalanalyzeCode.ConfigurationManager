using System.Diagnostics;
using System.Net.Http.Json;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests;

[Collection("Test collection")]
public class GetAppSettingEndpointTests : TestBase, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly Func<Task> _resetDatabase;

    public GetAppSettingEndpointTests(ApiWebApplication factory)
    {
        _client = factory.HttpClient;
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
        var settings = await _client.GetFromJsonAsync<ResponseDataModel<GetAppSettingsResponse>>($"{ProjectConstant.GetAppSettings}?settingName=TestSettings");

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
        var settings = await _client.GetFromJsonAsync<ResponseDataModel<GetAppSettingsResponse>>($"{ProjectConstant.GetAppSettings}?settingName=invalidName");

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