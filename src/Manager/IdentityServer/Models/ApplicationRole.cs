using Identity.Shared.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models;

public class ApplicationRole : IdentityRole
{
    public Permissions Permissions { get; set; }
}