using System.Diagnostics;
using System.Net.Http.Json;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Response;

namespace Api.IntegrationTests;

public class GetAppSettingEndpoint : TestBase
{
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenValidSettingNamesParse()
    {
        // Arrange
        var client = Application.CreateClient();

        // Act
        var settings = await client.GetFromJsonAsync<ResponseDataModel<GetAppSettingsResponse>>("/api/appsettings?settingName=StarfishOptions");

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
        var client = Application.CreateClient();

        // Act
        var settings = await client.GetFromJsonAsync<ResponseDataModel<GetAppSettingsResponse>>("/api/appsettings?settingName=invalidName");

        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        settings.Success.Should().BeFalse();
        settings.Data.Should().NotBeNull();
        Debug.Assert(settings.Data is not null);
        settings.Data.Settings.Should().BeNullOrEmpty();
    }
}