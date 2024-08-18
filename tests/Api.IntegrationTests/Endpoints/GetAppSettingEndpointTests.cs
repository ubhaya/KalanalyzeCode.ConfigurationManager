using System.Diagnostics;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints;

[Collection(Collections.ApiWebApplicationCollection)]
public class GetAppSettingEndpointTests : TestBase
{
    private readonly IMediator _mediator;

    public GetAppSettingEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    
    
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenValidSettingNamesParse()
    {
        // Arrange
        var testSettings = "TestSettings";
        await AddAsync(new ConfigurationSettings() { Id = testSettings, Value = "true" });
        var request = new GetAppSettingsRequest()
        {
            SettingName = testSettings
        };

        // Act
        var settings = await _mediator.Send(request, CancellationToken);
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
        var request = new GetAppSettingsRequest()
        {
            SettingName = "invalidName"
        };
    
        // Act
        var settings = await _mediator.Send(request, CancellationToken);
    
        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        Debug.Assert(settings.Settings is not null);
        settings.Settings.Should().BeNullOrEmpty();
    }
}