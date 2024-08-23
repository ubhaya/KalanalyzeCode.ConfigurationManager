using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
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
        var testSettings = "Project 1";
        var project = Fixture.Build<Project>()
            .With(p=>p.Name, testSettings)
            .Create();
        await AddAsync(project);
        var request = new GetAppSettingsRequest()
        {
            ProjectName = testSettings
        };

        // Act
        var settings = await _mediator.Send(request, CancellationToken);
        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        Debug.Assert(settings.Configurations is not null);
        settings.Configurations.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetAppSettingEndpoint_ReturnResult_WhenInvalidSettingNamesParse()
    {
        // Arrange
        var request = new GetAppSettingsRequest()
        {
            ProjectName = "invalidName"
        };
    
        // Act
        var settings = await _mediator.Send(request, CancellationToken);
    
        // Assert
        settings.Should().NotBeNull();
        Debug.Assert(settings is not null);
        Debug.Assert(settings.Configurations is not null);
        settings.Configurations.Should().BeNullOrEmpty();
    }
}