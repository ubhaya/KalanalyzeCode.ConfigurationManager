using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection("Test collection")]
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
        var allProjectResponse = await _client.GetAllAsync(CancellationToken);

        // Assert
        allProjectResponse.Should().NotBeNull();
        allProjectResponse.Data.Should().NotBeNull();
        allProjectResponse.Data.Projects.Should().BeEmpty();
    }
}