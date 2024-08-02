using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class GetGetAllEndpointsTests : TestBase
{
    private readonly IProjectsClient _client;

    public GetGetAllEndpointsTests(ApiWebApplication factory) : base(factory)
    {
        _client = new ProjectsClient(factory.HttpClient);
    }

    [Fact]
    public async Task GetAllProjects_ShouldReturnEmptyList_WhenNoProjectInDatabase()
    {
        // Arrange
        
        // Act
        var allProjectResponse =
            await _client.GetAllAsync(string.Empty, 0, 10, CustomSortDirection.None, string.Empty, CancellationToken);

        // Assert
        allProjectResponse.Should().NotBeNull();
        allProjectResponse.Projects.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllProjects_ShouldReturnAllTheProjects_WhenProjectsInDatabase()
    {
        // Arrange
        var projectsInDatabase = Fixture.Build<Entity.Entities.Project>()
            .CreateMany(100)
            .ToList();
        var projectList = projectsInDatabase.Select(x => new Project()
        {
            Name = x.Name,
            Id = x.Id
        });
        await AddRangeAsync(projectsInDatabase);
            
        
        // Act
        var allProjectResponse =
            await _client.GetAllAsync(string.Empty, 0, 10, CustomSortDirection.None, string.Empty, CancellationToken);

        // Assert
        allProjectResponse.Should().NotBeNull();
        allProjectResponse.Projects.Should().NotBeNull();
        //allProjectResponse.Data.Projects.Should().AllBeEquivalentTo(projectList);
    }
}