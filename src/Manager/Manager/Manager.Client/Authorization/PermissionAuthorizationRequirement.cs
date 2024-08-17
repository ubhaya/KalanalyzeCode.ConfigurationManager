using Microsoft.AspNetCore.Authorization;

namespace KalanalyzeCode.ConfigurationManager.Ui.Client.Authorization;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public PermissionAuthorizationRequirement(Permissions permission)
    {
        Permission = permission;
    }
    
    public Permissions Permission { get; }
}