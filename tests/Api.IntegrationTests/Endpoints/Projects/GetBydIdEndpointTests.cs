using System.Diagnostics;
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
        await AddAsync(projectInDatabase);
        var request = new GetProjectByIdRequest(projectInDatabase.Id);
        
        // Act
        var project = await _mediator.Send(request, CancellationToken);

        // Assert
        project.Should().NotBeNull();
        project.Project.Should().NotBeNull();
        project.Project.Should().BeEquivalentTo(projectInDatabase);
    }
    
    [Fact]
    public async Task GetProjectById_ReturnResultWithoutApiKey_WhenValidProjectIdInDatabaseAndApiKeyNotAssigned()
    {
        // Arange
        var id = Guid.NewGuid();
        var projectInDatabase = Fixture.Build<Project>()
            .With(x => x.Id, id)
            .Without(x=>x.ApiKey)
            .Create();
        await AddAsync(projectInDatabase);
        var request = new GetProjectByIdRequest(id);

        // Act
        var response = await _mediator.Send(request, CancellationToken);

        // Assert
        // var project = response.Match(x => x,
        //     () => throw new NullReferenceException());

        var project = response.Project;
        
        Debug.Assert(project is not null);
        
        project.Should().NotBeNull();
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(projectInDatabase.ApiKey);
    }
    
    [Fact]
    public async Task GetProjectById_ReturnResultWithApiKey_WhenValidProjectIdInDatabaseAndApiKeyAssigned()
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
        var request = new GetProjectByIdRequest(id);

        // Act
        var response = await _mediator.Send(request, CancellationToken);

        // Assert
        // var project = response.Match(x => x,
        //     () => throw new NullReferenceException());

        var project = response.Project;
        
        Debug.Assert(project is not null);
        project.Should().NotBeNull();
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(projectInDatabase.ApiKey);
    }    
    
    [Fact]
    public async Task GetProjectById_ShouldReturnResultWithConfiguration_WhenValidConfigurationInDataBase()
    {
        // Arange
        var projectId = Guid.NewGuid();
        var projectInDatabase = Fixture.Build<Project>()
            .With(x => x.Id, projectId)
            .Without(x=>x.Configurations)
            .Create();
        var configuration = Fixture.Build<Configuration>()
            .With(x => x.ProjectId, projectId)
            .Without(x=>x.Project)
            .CreateMany(10)
            .ToList();
        await AddAsync(projectInDatabase);
        await AddRangeAsync(configuration);

        // Act
        var result = await _mediator.Send(new GetProjectByIdRequest(projectId), CancellationToken);

        // Assert
        // var project = response.Match(x => x,
        //     () => throw new NullReferenceException());

        var project = result.Project;

        project.Should().NotBeNull();
        Debug.Assert(project is not null);
        project.Id.Should().Be(projectInDatabase.Id);
        project.Name.Should().Be(projectInDatabase.Name);
        project.ApiKey.Should().Be(projectInDatabase.ApiKey);
        project.Configurations.Should().BeEquivalentTo(configuration);
    }
}