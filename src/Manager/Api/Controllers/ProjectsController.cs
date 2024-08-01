using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Api.Controllers;

public class ProjectsController : ApiControllerBase
{
    public ProjectsController(ISender mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Permissions.Project | Permissions.Read)]
    public async Task<ActionResult<ResponseDataModel<GetAllProjectsResponse>>> GetAllAsync(
        [FromQuery] GetAllProjectsRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Permissions.Project | Permissions.Read)]
    public async Task<ActionResult<ResponseDataModel<GetProjectByIdResponse>>> GetByIdAsync(
        [FromRoute] GetProjectByIdRequest request, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Permissions.Project | Permissions.Write)]
    public async Task<ActionResult<ResponseDataModel<CreateProjectResponse>>> PostAsync(
        [FromBody] CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Permissions.Project | Permissions.Write)]
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
    public async Task<IActionResult> DeleteMotor([FromRoute] DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return NoContent();
    }
}