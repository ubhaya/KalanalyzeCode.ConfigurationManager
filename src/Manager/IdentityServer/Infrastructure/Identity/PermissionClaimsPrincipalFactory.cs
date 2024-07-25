using System.Security.Claims;
using Identity.Shared.Authorization;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityServer.Infrastructure.Identity;

public class PermissionClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    public PermissionClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager, 
        IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var userRoleName = await UserManager.GetRolesAsync(user);
        var userRoles = await RoleManager.Roles.Where(r =>
            userRoleName.Contains(r.Name ?? string.Empty)).ToListAsync();

        var userPermissions = Permissions.None;

        foreach (var role in userRoles)
            userPermissions |= role.Permissions;

        var permissionValue = (int)userPermissions;
        
        identity.AddClaim(new Claim(CustomClaimTypes.Permissions, permissionValue.ToString()));
        return identity;
    }
}