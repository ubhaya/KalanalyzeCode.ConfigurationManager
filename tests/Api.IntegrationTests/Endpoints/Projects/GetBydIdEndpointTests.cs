using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection("Test collection")]
public class GetBydIdEndpointTests : TestBase, IAsyncLifetime
{
    private readonly IProjectsClient _client;
    private readonly IServiceScope _scope;
    private readonly Func<Task> _resetDatabase;
    private CancellationTokenSource? _cancellationTokenSource;
    private CancellationToken _cancellationToken;

    public GetBydIdEndpointTests(ApiWebApplication factory)
    {
        _client = new ProjectsClient(factory.HttpClient);
        _scope = factory.Scope;
        _resetDatabase = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task GetProjectById_ShouldReturn404_WhenNoProjectInDatabase()
    {
        // Arrange
        
        // Act
        var project = await _client.GetByIdAsync(Guid.NewGuid(), _cancellationToken);

        // Assert
        project.Should().NotBeNull();
        project.Data.Should().NotBeNull();
        project.Data.Project.Should().BeNull();
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