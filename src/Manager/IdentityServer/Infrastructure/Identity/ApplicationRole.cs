using IdentityServer.Shared.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Infrastructure.Identity;

public class ApplicationRole : IdentityRole
{
    public Permissions Permission { get; set; }
}