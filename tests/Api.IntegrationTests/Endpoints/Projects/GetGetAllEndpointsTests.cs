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
        
        await AddRangeAsync(projectsInDatabase);
        var request = new GetAllProjectsRequest(string.Empty, 0, 100, CustomSortDirection.None, string.Empty);
        
        // Act
        var allProjectResponse =
            await _mediator.Send(request, CancellationToken);

        // Assert
        allProjectResponse.Should().NotBeNull();
        allProjectResponse.Projects.Should().NotBeNull();
        allProjectResponse.Projects.Should().BeEquivalentTo(projectsInDatabase);
    }

    [Fact]
    public async Task GetAllProject_ShouldReturnFilteredProject_WhenValidSearchString()
    {
        // Arange
        var seachString = "Project 1";
        var projects = Enumerable.Range(1, 10).Select(x => new Project()
        {
            Name = $"Project {x}",
        }).ToList();
        await AddRangeAsync(projects);
        var returnList = projects.Where(x=>x.Name.Contains(seachString, StringComparison.OrdinalIgnoreCase)).ToList();
        var request = new GetAllProjectsRequest(seachString, 0, 10, CustomSortDirection.None, string.Empty);

        // Act
        var response = await _mediator.Send(request, CancellationToken);
        
        // Assert
        response.Should().NotBeNull();
        response.TotalItem.Should().Be(returnList.Count);
        response.Projects.Should().NotBeNull();
        response.Projects.Count().Should().Be(returnList.Count);
    }
}