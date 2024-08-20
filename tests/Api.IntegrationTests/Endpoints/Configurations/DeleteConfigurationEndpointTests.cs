using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Configurations;

[Collection(Collections.ApiWebApplicationCollection)]
public class DeleteConfigurationEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    public DeleteConfigurationEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task DeleteConfigurationEndpoint_ShouldDeleteTheConfigurationInDatabase_WhenTheItemExists()
    {
        // Arange
        var projectWithConfiguration = Fixture.Build<Project>()
            .Create();
        var configurationToDelete = projectWithConfiguration.Configurations.FirstOrDefault();
        Debug.Assert(configurationToDelete is not null);
        await AddAsync(projectWithConfiguration);
        var request = new DeleteConfigurationRequest(configurationToDelete.Id);
        var getRequest = new GetProjectByIdRequest(projectWithConfiguration.Id);

        // Act
        await _mediator.Send(request, CancellationToken);
        var projectResponse = await _mediator.Send(getRequest, CancellationToken);

        // Assert
        projectResponse.Should().NotBeNull();
        var project = projectResponse.Project;
        Debug.Assert(project is not null);
        project.Configurations.Should().NotContain(configurationToDelete);
    }
}