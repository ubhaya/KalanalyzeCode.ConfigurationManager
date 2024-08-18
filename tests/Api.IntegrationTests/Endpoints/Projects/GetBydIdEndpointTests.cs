using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class GetBydIdEndpointTests : TestBase
{
    private readonly IMediator _mediator;

    public GetBydIdEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task GetProjectById_ShouldReturn404_WhenNoProjectInDatabase()
    {
        // Arrange
        var request = new GetProjectByIdRequest(Guid.NewGuid());
        
        // Act
        var project = await _mediator.Send(request, CancellationToken);

        // Assert
        project.Should().NotBeNull();
        project.Project.Should().BeNull();
    }
    
    [Fact]
    public async Task GetProjectById_ShouldReturnOneProject_WhenValidProjectInDatabase()
    {
        // Arrange
        var projectInDatabase = Fixture.Create<Project>();
        var projectToMatch = new Project()
        {
            Id = projectInDatabase.Id,
            Name = projectInDatabase.Name,
            ApiKey = projectInDatabase.ApiKey
        };
        await AddAsync(projectInDatabase);
        var request = new GetProjectByIdRequest(projectInDatabase.Id);
        
        // Act
        var project = await _mediator.Send(request, CancellationToken);

        // Assert
        project.Should().NotBeNull();
        project.Project.Should().NotBeNull();
        project.Project.Should().BeEquivalentTo(projectToMatch);
    }
}