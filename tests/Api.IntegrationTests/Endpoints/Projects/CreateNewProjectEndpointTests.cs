using System.Diagnostics;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class CreateNewProjectEndpointTests : TestBase
{
    private readonly IMediator _mediator;

    public CreateNewProjectEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task PostProject_ShouldCreateAProjectInDatabase_WhenValidProjectNameReceived()
    {
        // Arrange
        var projectToCreate = new CreateProjectRequest()
        {
            ProjectName = "Test Project"
        };
        
        // Act
        var createdProjectResponse = await _mediator.Send(projectToCreate, CancellationToken);

        // Assert
        createdProjectResponse.Should().NotBeNull();
        var createdProject = createdProjectResponse.Project;
        createdProject.Should().NotBeNull();
        Debug.Assert(createdProject is not null);
        projectToCreate.ProjectName.Should().Be(createdProject.Name);
        createdProject.Id.Should().Be(createdProject.Id);
    }
}