using System.Diagnostics;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class CreateNewProjectEndpointTests : TestBase
{
    private readonly IProjectsClient _client;

    public CreateNewProjectEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _client = new ProjectsClient(factory.HttpClient);
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
        var result = await _client.PostAsync(projectToCreate, CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().NotBeEmpty();
        var createdProject = await Context.Projects.FindAsync([result.Data.Id], CancellationToken);
        createdProject.Should().NotBeNull();
        Debug.Assert(createdProject is not null);
        projectToCreate.ProjectName.Should().Be(createdProject.Name);
        result.Data.Id.Should().Be(createdProject.Id);
    }
}