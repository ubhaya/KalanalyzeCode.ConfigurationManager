using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Projects;

[Collection(Collections.ApiWebApplicationCollection)]
public class GetGetAllEndpointsTests : TestBase
{
    private readonly IMediator _mediator;

    public GetGetAllEndpointsTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }

    [Fact]
    public async Task GetAllProjects_ShouldReturnEmptyList_WhenNoProjectInDatabase()
    {
        // Arrange
        var request = new GetAllProjectsRequest(string.Empty, 0, 10, CustomSortDirection.None, string.Empty);
        
        // Act
        var allProjectResponse =
            await _mediator.Send(request, CancellationToken);

        // Assert
        allProjectResponse.Should().NotBeNull();
        allProjectResponse.Projects.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllProjects_ShouldReturnAllTheProjects_WhenProjectsInDatabase()
    {
        // Arrange
        var projectsInDatabase = Fixture.Build<Project>()
            .CreateMany(100)
            .ToList();
        var projectList = projectsInDatabase.Select(x => new Project()
        {
            Name = x.Name,
            Id = x.Id
        });
        await AddRangeAsync(projectsInDatabase);
        var request = new GetAllProjectsRequest(string.Empty, 0, 10, CustomSortDirection.None, string.Empty);
        
        // Act
        var allProjectResponse =
            await _mediator.Send(request, CancellationToken);

        // Assert
        allProjectResponse.Should().NotBeNull();
        allProjectResponse.Projects.Should().NotBeNull();
        //allProjectResponse.Data.Projects.Should().AllBeEquivalentTo(projectList);
    }
}