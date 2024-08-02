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
        var idInDatabase = Guid.NewGuid();
        var projectToUpdated = new UpdateProjectRequest()
        {
            ProjectName = "Updated Name",
            Id = idInDatabase,
        };
        var projectInDatabase = new Entity.Entities.Project()
        {
            Name = "Old Name",
            Id = idInDatabase
        };
        await AddAsync(projectInDatabase);
        
        // Act
        await _client.PutAsync(idInDatabase, projectToUpdated, CancellationToken);
        var updatedProjectResponse = await _client.GetByIdAsync(idInDatabase, CancellationToken);

        // Assert
        updatedProjectResponse.Should().NotBeNull();
        var updatedProject = updatedProjectResponse.Project;
        updatedProject.Should().NotBeNull();
        Debug.Assert(updatedProject is not null);
        updatedProject.Id.Should().Be(idInDatabase);
        updatedProject.Name.Should().Be(projectToUpdated.ProjectName);
        updatedProject.Name.Should().NotBe(projectInDatabase.Name);
    }
}