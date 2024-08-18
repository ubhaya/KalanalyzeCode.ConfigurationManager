using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class DeleteProjectEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    public DeleteProjectEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task DeleteProject_ShouldDeleteTheProjectInDatabase_WhenTheItemExistInDatabase()
    {
        // Arange
        var id = Guid.NewGuid();
        var project = new Entity.Entities.Project
        {
            Id = id,
            Name = "Project Deleted"
        };
        await AddAsync(project);
        var request = new DeleteProjectRequest(id);
        var getRequest = new GetProjectByIdRequest(id);

        // Act
        await _mediator.Send(request, CancellationToken);
        var deleteProjectResponse = await _mediator.Send(getRequest, CancellationToken);

        // Assert
        deleteProjectResponse.Should().NotBeNull();
        var deletedProject = deleteProjectResponse.Project;
        deletedProject.Should().BeNull();
    }
}