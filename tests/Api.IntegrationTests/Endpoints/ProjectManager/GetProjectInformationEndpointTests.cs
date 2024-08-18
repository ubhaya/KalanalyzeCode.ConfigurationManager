using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ProjectManager;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.ProjectManager;

[Collection(Collections.ApiWebApplicationCollection)]
public sealed class GetProjectInformationEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    
    public GetProjectInformationEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task GetProjectInformationEndpoint_ReturnResultWithoutApiKey_WhenValidProjectIdInDatabaseAndApiKeyNotAssigned()
    {
        // Arange
        var id = Guid.NewGuid();
        var projectInDatabase = Fixture.Build<Project>()
            .With(x => x.Id, id)
            .Without(x=>x.ApiKey)
            .Create();
        await AddAsync(projectInDatabase);
        var request = new GetProjectInformationRequest(id);

        // Act
        var response = await _mediator.Send(request, CancellationToken);

        // Assert
        var project = response.Match(x => x,
            () => throw new NullReferenceException());
        
        project.Should().NotBeNull();
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(projectInDatabase.ApiKey);
    }
    
    [Fact]
    public async Task GetProjectInformationEndpoint_ReturnResultWithApiKey_WhenValidProjectIdInDatabaseAndApiKeyAssigned()
    {
        // Arange
        var id = Guid.NewGuid();
        var projectInDatabase = Fixture.Build<Project>()
            .With(x => x.Id, id)
            .Create();
        var projectToMatch = new Project()
        {
            Name = projectInDatabase.Name,
            Id = projectInDatabase.Id
        };
        await AddAsync(projectInDatabase);
        var request = new GetProjectInformationRequest(id);

        // Act
        var response = await _mediator.Send(request, CancellationToken);

        // Assert
        var project = response.Match(x => x,
            () => throw new NullReferenceException());
        project.Should().NotBeNull();
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(projectInDatabase.ApiKey);
    }
}