using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Api.Controllers;

public class ApiKeyManagerController : ApiControllerBase
{
    public ApiKeyManagerController(ISender mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    [Authorize(Permissions.ProjectWrite)]
    public async Task<IActionResult> PostAsync([FromBody] CreateApiKeyForProjectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return result.Match<IActionResult>(s => Ok(s),
            BadRequest);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Permissions.ProjectWrite)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Mediator.Send(new DeleteApiKeyForProjectRequest(id), cancellationToken);
        return NoContent();
    }
}