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
        project.Data.Should().NotBeNull();
        project.Data.Project.Should().BeNull();
    }
    
    [Fact]
    public async Task GetProjectById_ShouldReturnOneProject_WhenValidProjectInDatabase()
    {
        // Arrange
        var projectInDatabase = Fixture.Create<Entity.Entities.Project>();
        var projectToMatch = new Project()
        {
            Id = projectInDatabase.Id,
            Name = projectInDatabase.Name
        };
        await AddAsync(projectInDatabase);
        
        // Act
        var project = await _client.GetByIdAsync(projectInDatabase.Id, CancellationToken);

        // Assert
        project.Should().NotBeNull();
        project.Data.Should().NotBeNull();
        project.Data.Project.Should().NotBeNull();
        project.Data.Project.Should().BeEquivalentTo(projectToMatch);
    }
}