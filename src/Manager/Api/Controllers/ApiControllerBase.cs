using Identity.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ISender Mediator;

    protected ApiControllerBase(ISender mediator)
    {
        Mediator = mediator;
    }
}