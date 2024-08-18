using System.Diagnostics;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Queries;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class UpdateProjectEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    public UpdateProjectEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task PutProject_ShouldEditTheProjectInDatabase_WhenTheItemExistInDatabase()
    {
        // Arrange
        var idInDatabase = Guid.NewGuid();
        var projectToUpdated = new UpdateProjectRequest()
        {
            ProjectName = "Updated Name",
            Id = idInDatabase,
        };
        var projectInDatabase = new Project()
        {
            Name = "Old Name",
            Id = idInDatabase
        };
        await AddAsync(projectInDatabase);
        var request = new GetProjectByIdRequest(idInDatabase);
        
        // Act
        await _mediator.Send(projectToUpdated, CancellationToken);
        var updatedProjectResponse = await _mediator.Send(request, CancellationToken);

        // Assert
        updatedProjectResponse.Should().NotBeNull();
        var updatedProject = updatedProjectResponse.Project;
        updatedProject.Should().NotBeNull();
        Debug.Assert(updatedProject is not null);
        updatedProject.Id.Should().Be(idInDatabase);
        updatedProject.Name.Should().Be(projectToUpdated.ProjectName);
    }
}