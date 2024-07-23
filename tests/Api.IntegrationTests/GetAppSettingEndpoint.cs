using System.Net.Http.Json;
using FluentAssertions;
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
        var settings = await client.GetFromJsonAsync<IEnumerable<ApplicationSettings>>("/appsettings?settingName=StarfishOptions");

        // Assert
        settings.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenInvalidSettingNamesParse()
    {
        // Arrange
        var client = Application.CreateClient();

        // Act
        var settings = await client.GetFromJsonAsync<List<ApplicationSettings>>("/appsettings?settingName=invalidName");

        // Assert
        settings.Should().NotBeNull();
        settings.Should().BeEmpty();
    }
}