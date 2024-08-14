using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.ApiKeyManager;

[Collection(Collections.ApiWebApplicationCollection)]
public class DeleteApiKeyEndpointTests : TestBase
{
    private readonly IApiKeyManagerClient _client;
    private readonly IProjectManagerClient _projectManagerClient;
    
    public DeleteApiKeyEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _client = new ApiKeyManagerClient(factory.HttpClient);
        _projectManagerClient = new ProjectManagerClient(factory.HttpClient);
    }

    [Fact]
    public async Task DeleteAPiKeyEndpoint_ShouldDeleteRemoveTheKeyFromProject_WhenProjectContainAKey()
    {
        // Arange
        var projectInDatabase = Fixture.Create<Entity.Entities.Project>();
        await AddAsync(projectInDatabase);

        // Act
        await _client.DeleteAsync(projectInDatabase.Id, CancellationToken);
        var deletedKeyProject = await _projectManagerClient.GetByIdAsync(projectInDatabase.Id, CancellationToken);

        // Assert
        deletedKeyProject.Should().NotBeNull();
        var project = deletedKeyProject.Project;
        project.Should().NotBeNull();
        project.ApiKey.Should().NotBe(projectInDatabase.ApiKey);
        project.ApiKey.Should().Be(Guid.Empty);
    }
}