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
public class CreateConfigurationEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    
    public CreateConfigurationEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task CreateConfiguration_ShouldCreateAConfigurationInDatabase_WhenValidConfigurationNameReceived()
    {
        // Arange
        var project = Fixture.Build<Project>()
            .Without(x => x.Configurations)
            .Create();
        var configurationRequest = Fixture.Build<CreateConfigurationRequest>()
            .With(x => x.ProjectId, project.Id)
            .Create();
        await AddAsync(project);
        var getRequest = new GetProjectByIdRequest(project.Id);

        // Act
        var createdConfigurationResponse = await _mediator.Send(configurationRequest, CancellationToken);
        var projectResponse = await _mediator.Send(getRequest, CancellationToken);

        // Assert
        var projectInDatabase = projectResponse.Project;
        projectInDatabase.Should().NotBeNull();
        Debug.Assert(projectInDatabase is not null);
        projectInDatabase.Id.Should().Be(project.Id);
        projectInDatabase.Name.Should().Be(project.Name);
        projectInDatabase.Configurations.Should().HaveCount(1);
        projectInDatabase.Configurations.First().ProjectId.Should().Be(project.Id);
        projectInDatabase.Configurations.First().Name.Should().Be(configurationRequest.Name);
        projectInDatabase.Configurations.First().Value.Should().Be(configurationRequest.Value);
    }
}