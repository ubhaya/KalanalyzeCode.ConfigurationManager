using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using KalanalyzeCode.ConfigurationManager.Ui.Client.Authorization;
using KalanalyzeCode.ConfigurationManager.Ui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Ui;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ProfileService(UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var subjectId = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(subjectId);
        if (user is null)
            return;
        var userRolesName = await _userManager.GetRolesAsync(user);

        var userRoles = await _roleManager.Roles.Where(r =>
            userRolesName.Contains(r.Name ?? string.Empty)).ToListAsync();

        var userPermissions = Permissions.None;

        foreach (var role in userRoles)
            userPermissions |= role.Permissions;

        var permissionValue = (int)userPermissions;

        var claims = context.Subject.Claims.Where(c => c.Type is 
            JwtClaimTypes.Name or JwtClaimTypes.Email);
        
        context.IssuedClaims.AddRange(claims);
        context.IssuedClaims.Add(new Claim(CustomClaimTypes.Permissions,permissionValue.ToString()));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        // todo: Implement IsActive()
        return Task.CompletedTask;
    }
}