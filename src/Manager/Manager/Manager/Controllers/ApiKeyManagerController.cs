using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Ui.Controllers;

public class ApiKeyManagerController : ApiControllerBase
{
    public ApiKeyManagerController(ISender mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.ProjectWrite)]
    public async Task<IActionResult> PostAsync([FromBody] CreateApiKeyForProjectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return result.Match<IActionResult>(s => Ok(s),
            BadRequest);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.ProjectWrite)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Mediator.Send(new DeleteApiKeyForProjectRequest(id), cancellationToken);
        return NoContent();
    }
}