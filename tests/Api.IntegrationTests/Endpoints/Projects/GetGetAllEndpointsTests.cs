using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection("Test collection")]
public class GetGetAllEndpointsTests : TestBase, IAsyncLifetime
{
    private readonly IProjectsClient _client;
    private readonly IServiceScope _scope;
    private readonly Func<Task> _resetDatabase;
    private CancellationTokenSource? _cancellationTokenSource;
    private CancellationToken _cancellationToken;

    public GetGetAllEndpointsTests(ApiWebApplication factory)
    {
        _client = new ProjectsClient(factory.HttpClient);
        _scope = factory.Scope;
        _resetDatabase = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task GetAllProjects_ShouldReturnEmptyList_WhenNoProjectInDatabase()
    {
        // Arrange
        
        // Act
        var allProjectResponse = await _client.GetAllAsync(_cancellationToken);

        // Assert
        allProjectResponse.Should().NotBeNull();
        allProjectResponse.Data.Should().NotBeNull();
        allProjectResponse.Data.Projects.Should().BeEmpty();
    }

    public Task InitializeAsync()
    {
        _cancellationToken = (_cancellationTokenSource ?? new CancellationTokenSource()).Token;
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        if (_cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            return _resetDatabase();
        }

        return _resetDatabase();
    }
}