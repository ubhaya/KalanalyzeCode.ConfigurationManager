using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.ApiKeyManager;

[Collection(Collections.ApiWebApplicationCollection)]
public sealed class PostApiKeyEndpointTests : TestBase
{
    private readonly IApiKeyManagerClient _client;
    private readonly IProjectManagerClient _projectManagerClient;
    public PostApiKeyEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _client = new ApiKeyManagerClient(factory.HttpClient);
        _projectManagerClient = new ProjectManagerClient(factory.HttpClient);
    }

    [Fact]
    public async Task PostApiKeyEndpoint_ShouldCreateANewApiKeyUser_WhenProjectNotContainAKey()
    {
        // Arange
        var projectInDatabase = Fixture.Build<Entity.Entities.Project>()
            .Without(x => x.ApiKey)
            .Create();
        await AddAsync(projectInDatabase);

        // Act
        var result = await _client.PostAsync(new CreateApiKeyForProjectRequest()
        {
            ProjectId = projectInDatabase.Id
        });
        var updatedProjectResponse = await _projectManagerClient.GetByIdAsync(projectInDatabase.Id, CancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        var project = updatedProjectResponse.Project;
        project.Should().NotBeNull();
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(result);
    }
    
    [Fact]
    public async Task PostApiKeyEndpoint_ShouldReturnTheSameApiKey_WhenProjectContainAKey()
    {
        // Arange
        var projectInDatabase = Fixture.Build<Entity.Entities.Project>()
            .Create();
        await AddAsync(projectInDatabase);

        // Act
        var result = await _client.PostAsync(new CreateApiKeyForProjectRequest()
        {
            ProjectId = projectInDatabase.Id
        });
        var updatedProjectResponse = await _projectManagerClient.GetByIdAsync(projectInDatabase.Id, CancellationToken);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Be(projectInDatabase.ApiKey);
        var project = updatedProjectResponse.Project;
        project.Should().NotBeNull();
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(result);
    }
}