using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class GetBydIdEndpointTests : TestBase
{
    private readonly IProjectsClient _client;

    public GetBydIdEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _client = new ProjectsClient(factory.HttpClient);
    }

    [Fact]
    public async Task GetProjectById_ShouldReturn404_WhenNoProjectInDatabase()
    {
        // Arrange
        
        // Act
        var project = await _client.GetByIdAsync(Guid.NewGuid(), CancellationToken);

        // Assert
        project.Should().NotBeNull();
        project.Project.Should().BeNull();
    }
    
    [Fact]
    public async Task GetProjectById_ShouldReturnOneProject_WhenValidProjectInDatabase()
    {
        // Arrange
        var projectInDatabase = Fixture.Create<Entity.Entities.Project>();
        var projectToMatch = new Project()
        {
            Id = projectInDatabase.Id,
            Name = projectInDatabase.Name,
            ApiKey = projectInDatabase.ApiKey
        };
        await AddAsync(projectInDatabase);
        
        // Act
        var project = await _client.GetByIdAsync(projectInDatabase.Id, CancellationToken);

        // Assert
        project.Should().NotBeNull();
        project.Project.Should().NotBeNull();
        project.Project.Should().BeEquivalentTo(projectToMatch);
    }
}