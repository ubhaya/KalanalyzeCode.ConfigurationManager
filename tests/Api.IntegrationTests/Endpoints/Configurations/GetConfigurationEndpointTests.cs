using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.Configurations;

[Collection(Collections.ApiWebApplicationCollection)]
public class GetConfigurationEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    
    public GetConfigurationEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
    }
    
    [Fact]
    public async Task GetConfigurationEndpoint_ShouldNotReturnResultWithConfiguration_WhenValidConfigurationInDataBase()
    {
        // Arange
        var projectId = Guid.NewGuid();
        var projectInDatabase = Fixture.Build<Project>()
            .With(x => x.Id, projectId)
            .Without(x=>x.Configurations)
            .Create();
        var configurationsInDatabase = Fixture.Build<Configuration>()
            .With(x => x.ProjectId, projectId)
            .Without(x=>x.Project)
            .CreateMany(10)
            .ToList();
        await AddAsync(projectInDatabase);
        await AddRangeAsync(configurationsInDatabase);

        // Act
        var result = await _mediator.Send(
            new GetAllConfigurationRequest(string.Empty, 0, 10, 
                CustomSortDirection.None, string.Empty, projectId));

        // Assert
        var configurations = result.Configurations.ToList();

        configurations.Should().NotBeEmpty();
        configurations.Should().BeEquivalentTo(configurationsInDatabase);
    }

    [Fact]
    public async Task GetConfigurationEndpoint_ShouldReturnAllTheConfiguration_WhenConfigurationInDatabase()
    {
        // Arange
        var searchString = "Project 2";
        var project = Fixture.Create<Project>();
        var configurations = Enumerable.Range(1, 100).Select(x => new Configuration()
        {
            Name = $"Project {x}",
            Value = $"Value {x}",
            ProjectId = project.Id,
        }).ToList();
        await AddAsync(project);
        await AddRangeAsync(configurations);
        var returnList = configurations.Where(x => x.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            .ToList();
        var request =
            new GetAllConfigurationRequest(searchString, 0, 100, CustomSortDirection.None, string.Empty, project.Id);

        // Act
        var response = await _mediator.Send(request, CancellationToken);

        // Assert
        response.Should().NotBeNull();
        response.TotalItems.Should().Be(returnList.Count);
        response.Configurations.Should().NotBeNull();
        response.Configurations.Should().BeEquivalentTo(returnList);
    }
}