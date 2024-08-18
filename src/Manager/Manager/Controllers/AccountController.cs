using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Ui.Controllers;

[ApiController]
[Route("/")]
public sealed class AccountController : ControllerBase
{
    [HttpPost("logout")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        return NoContent(); // Or return an appropriate response
    }

    [HttpGet("login")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task Login([FromQuery] string returnUrl)
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = returnUrl
        };
        await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
    }
}