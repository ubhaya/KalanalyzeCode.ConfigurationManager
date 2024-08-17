using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Ui.Controllers;

public class ProjectsController : ApiControllerBase
{
    public ProjectsController(ISender mediator) : base(mediator)
    {
    }

    [HttpGet]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.ProjectRead)]
    public async Task<ActionResult<GetAllProjectsResponse>> GetAllAsync(
        [FromQuery] GetAllProjectsRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.ProjectRead)]
    [ProducesResponseType<GetProjectByIdResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetProjectByIdRequest(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.ProjectWrite)]
    [ProducesResponseType<Project>(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostAsync(
        [FromBody] CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction("GetById", new {id = result.Project?.Id}, result.Project);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.ProjectWrite)]
    public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateProjectRequest request,
        CancellationToken cancellationToken = default)
    {
        if (id != request.Id) return BadRequest();

        await Mediator.Send(request, cancellationToken);

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.ProjectWrite)]
    public async Task<IActionResult> DeleteAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        await Mediator.Send(new DeleteProjectRequest(id), cancellationToken);

        return NoContent();
    }
}