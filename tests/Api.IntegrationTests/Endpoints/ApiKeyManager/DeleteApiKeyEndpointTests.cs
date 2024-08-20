using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Common.Models;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Endpoints.ApiKeyManager;

[Collection(Collections.ApiWebApplicationCollection)]
public class DeleteApiKeyEndpointTests : TestBase
{
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public DeleteApiKeyEndpointTests(ApiWebApplication factory) : base(factory)
    {
        _mediator = factory.Mediator;
        _userManager = factory.Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    }

    [Fact]
    public async Task DeleteAPiKeyEndpoint_ShouldDeleteRemoveTheKeyFromProject_WhenProjectContainAKey()
    {
        // Arange
        var projectInDatabase = Fixture.Build<Project>()
            .With(x => x.ApiKey, Guid.NewGuid)
            .Create();
        var apiKey = projectInDatabase.ApiKey;
        var user = Fixture.Build<ApplicationUser>()
            .With(x => x.Id, projectInDatabase.ApiKey)
            .Without(x=>x.LockoutEnd)
            .Create();
        await AddAsync(projectInDatabase);
        await _userManager.CreateAsync(user, "Pass123$");
        var request = new DeleteApiKeyForProjectRequest(projectInDatabase.Id);
        var getRequest = new GetProjectByIdRequest(projectInDatabase.Id);

        // Act
        await _mediator.Send(request, CancellationToken);
        var deletedKeyProjectOption = await _mediator.Send(getRequest, CancellationToken);

        // Assert
        //var project = deletedKeyProjectOption.Match<Project?>(p => p, () => null);
        var project = deletedKeyProjectOption.Project;
        Debug.Assert(project is not null);
        project.Should().NotBeNull();
        Debug.Assert(project is not null);
        project.ApiKey.Should().NotBe(apiKey);
        project.ApiKey.Should().Be(Guid.Empty);
    }
}