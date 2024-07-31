using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection("Test collection")]
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
}