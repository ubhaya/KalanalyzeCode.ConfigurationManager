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
public class UpdateConfigurationEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    public UpdateConfigurationEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task
        UpdateConfigurationEndpoint_ShouldEditTheConfigurationInDatabase_WhenTheConfigurationExistsInTheDatabase()
    {
        // Arange
        var projectInDatabase = Fixture.Create<Project>();
        var updatedName = "UpdatedName";
        var updatedValue = "UpdatedValue";
        
        await AddAsync(projectInDatabase);
        var configurationToUpdate = projectInDatabase.Configurations.FirstOrDefault();
        Debug.Assert(configurationToUpdate is not null);
        var request = new UpdateConfigurationRequest(configurationToUpdate.Id, updatedName, updatedValue);
        var getRequest = new GetProjectByIdRequest(projectInDatabase.Id);

        // Act
        await _mediator.Send(request, CancellationToken);
        var updatedProject = await _mediator.Send(getRequest, CancellationToken);

        // Assert
        updatedProject.Should().NotBeNull();
        updatedProject.Project.Should().NotBeNull();
        Debug.Assert(updatedProject.Project is not null);
        var configurations = updatedProject.Project.Configurations;
        var updatedConfiguration =configurations.SingleOrDefault(x=>x.Id == configurationToUpdate.Id);
        updatedConfiguration.Should().NotBeNull();
        Debug.Assert(updatedConfiguration is not null);
        updatedConfiguration.Name.Should().Be(updatedName);
        updatedConfiguration.Value.Should().Be(updatedValue);
    }
}