using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ProjectManager;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.ApiKeyManager;

[Collection(Collections.ApiWebApplicationCollection)]
public sealed class PostApiKeyEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    public PostApiKeyEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task PostApiKeyEndpoint_ShouldCreateANewApiKeyUser_WhenProjectNotContainAKey()
    {
        // Arange
        var projectInDatabase = Fixture.Build<Entity.Entities.Project>()
            .Without(x => x.ApiKey)
            .Create();
        await AddAsync(projectInDatabase);
        var getRequest = new GetProjectInformationRequest(projectInDatabase.Id);

        // Act
        var result = await _mediator.Send(new CreateApiKeyForProjectRequest(projectInDatabase.Id), CancellationToken);
        var updatedProjectResponseOption = await _mediator.Send(getRequest, CancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var createdApiKey = result.Match(x => x,
            exception => throw exception);

        var project = updatedProjectResponseOption.Match(project => project, () => throw new NullReferenceException());
        project.Should().NotBeNull();
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(createdApiKey);  

    }
    
    [Fact]
    public async Task PostApiKeyEndpoint_ShouldReturnTheSameApiKey_WhenProjectContainAKey()
    {
        // Arange
        var projectInDatabase = Fixture.Build<Entity.Entities.Project>()
            .Create();
        await AddAsync(projectInDatabase);
        var getRequest = new GetProjectInformationRequest(projectInDatabase.Id);
    
        // Act
        var result = await _mediator.Send(new CreateApiKeyForProjectRequest(projectInDatabase.Id));
        var updatedProjectResponseOption = await _mediator.Send(getRequest, CancellationToken);
    
        // Assert
        result.IsSuccess.Should().BeTrue();

        var createdApiKey = result.Match(x => x,
            exception => throw exception);
        
        createdApiKey.Should().NotBeEmpty();
        createdApiKey.Should().Be(projectInDatabase.ApiKey);
        var project = updatedProjectResponseOption.Match(project => project, () => throw new NullReferenceException());
        project.Should().NotBeNull();
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(createdApiKey);
    }
}