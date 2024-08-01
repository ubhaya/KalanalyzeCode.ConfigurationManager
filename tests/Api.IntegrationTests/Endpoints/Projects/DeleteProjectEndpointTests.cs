using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class DeleteProjectEndpointTests : TestBase
{
    private readonly IProjectsClient _client;
    public DeleteProjectEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _client = new ProjectsClient(factory.HttpClient);
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

        // Act
        await _client.DeleteAsync(id, CancellationToken);
        var deleteProjectResponse = await _client.GetByIdAsync(id, CancellationToken);

        // Assert
        deleteProjectResponse.Should().NotBeNull();
        var deletedProject = deleteProjectResponse.Data.Project;
        deletedProject.Should().BeNull();
    }
}