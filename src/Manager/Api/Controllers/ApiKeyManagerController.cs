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
    public async Task<IActionResult> PostAsync([FromBody] CreateApiKeyForProjectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return result.Match<IActionResult>(s => Ok(s),
            BadRequest);
    }
}