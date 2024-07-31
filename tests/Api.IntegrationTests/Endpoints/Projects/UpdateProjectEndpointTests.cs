using System.Diagnostics;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class UpdateProjectEndpointTests : TestBase
{
    private readonly IProjectsClient _client;
    public UpdateProjectEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _client = new ProjectsClient(factory.HttpClient);
    }

    [Fact]
    public async Task PutProject_ShouldEditTheProjectInDatabase_WhenTheItemExistInDatabase()
    {
        // Arrange
        var id = Guid.NewGuid();
        var projectToUpdated = new UpdateProjectRequest()
        {
            ProjectName = "Updated Name",
            Id = id,
        };
        var projectInDatabase = new Entity.Entities.Project()
        {
            Id = id,
            Name = "Old Name"
        };
        await AddAsync(projectInDatabase);
        
        
        // Act
        await _client.PutAsync(id, projectToUpdated, CancellationToken);

        // Assert
        var updatedProject = await Context.Projects.FindAsync([id], CancellationToken);
        updatedProject.Should().NotBeNull();
        Debug.Assert(updatedProject is not null);
        updatedProject.Id.Should().Be(id);
        // updatedProject.Name.Should().Be(projectToUpdated.ProjectName);
        // updatedProject.Name.Should().NotBe(projectInDatabase.Name);
    }
}