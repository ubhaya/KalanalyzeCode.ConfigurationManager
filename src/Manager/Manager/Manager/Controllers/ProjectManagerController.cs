using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ProjectManager;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.ProjectManager;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Ui.Controllers;

public class ProjectManagerController : ApiControllerBase
{
    public ProjectManagerController(ISender mediator) : base(mediator)
    {
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<GetProjectInformationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.ProjectRead)]
    public async Task<ActionResult<GetProjectInformationResponse>> GetByIdAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetProjectInformationRequest(id), cancellationToken);

        return response.Match<ActionResult<GetProjectInformationResponse>>(p => Ok(new GetProjectInformationResponse
            {
                Project = p
            }),
            () => NotFound());
    }
}