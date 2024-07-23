using System.Diagnostics;
using System.Net.Http.Json;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared;

namespace Api.IntegrationTests;

public class GetAppSettingEndpoint : TestBase
{
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenValidSettingNamesParse()
    {
        // Arrange
        var client = Application.CreateClient();

        // Act
        var settings = await client.GetFromJsonAsync<ResponseDataModel<IEnumerable<ApplicationSettings>>>("/api/appsettings?settingName=StarfishOptions");

        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        settings.Success.Should().BeTrue();
        settings.Data.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenInvalidSettingNamesParse()
    {
        // Arrange
        var client = Application.CreateClient();

        // Act
        var settings = await client.GetFromJsonAsync<ResponseDataModel<IEnumerable<ApplicationSettings>>>("/api/appsettings?settingName=invalidName");

        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        settings.Success.Should().BeFalse();
        settings.Data.Should().BeNullOrEmpty();
    }
}