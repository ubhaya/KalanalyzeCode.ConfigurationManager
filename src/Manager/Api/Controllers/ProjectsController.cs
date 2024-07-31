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
        //[FromQuery] GetAllProjectsRequest request, 
        CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetAllProjectsRequest(), cancellationToken);
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
}