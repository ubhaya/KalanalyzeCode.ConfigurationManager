using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Client;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.ProjectManager;

[Collection(Collections.ApiWebApplicationCollection)]
public sealed class GetProjectInformationEndpointTests : TestBase
{
    private readonly IProjectManagerClient _client;
    
    public GetProjectInformationEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _client = new ProjectManagerClient(factory.HttpClient);
    }

    [Fact]
    public async Task GetProjectInformationEndpoint_ReturnResultWithoutApiKey_WhenValidProjectIdInDatabaseAndApiKeyNotAssigned()
    {
        // Arange
        var id = Guid.NewGuid();
        var projectInDatabase = Fixture.Build<Entity.Entities.Project>()
            .With(x => x.Id, id)
            .Without(x=>x.ApiKey)
            .Create();
        var projectToMatch = new Project()
        {
            Name = projectInDatabase.Name,
            Id = projectInDatabase.Id
        };
        await AddAsync(projectInDatabase);

        // Act
        var response = await _client.GetByIdAsync(id, CancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Project.Should().NotBeNull();
        response.Project.Id.Should().Be(projectInDatabase.Id);
        response.Project.Name.Should().Be(projectInDatabase.Name);
        response.Project.ApiKey.Should().Be(projectInDatabase.ApiKey);
    }
    
    [Fact]
    public async Task GetProjectInformationEndpoint_ReturnResultWithApiKey_WhenValidProjectIdInDatabaseAndApiKeyAssigned()
    {
        // Arange
        var id = Guid.NewGuid();
        var projectInDatabase = Fixture.Build<Entity.Entities.Project>()
            .With(x => x.Id, id)
            .Create();
        var projectToMatch = new Project()
        {
            Name = projectInDatabase.Name,
            Id = projectInDatabase.Id
        };
        await AddAsync(projectInDatabase);

        // Act
        var response = await _client.GetByIdAsync(id, CancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.Project.Should().NotBeNull();
        response.Project.Id.Should().Be(projectInDatabase.Id);
        response.Project.Name.Should().Be(projectInDatabase.Name);
        response.Project.ApiKey.Should().Be(projectInDatabase.ApiKey);
    }
}