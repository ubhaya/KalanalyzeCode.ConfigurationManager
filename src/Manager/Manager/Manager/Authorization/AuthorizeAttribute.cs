namespace KalanalyzeCode.ConfigurationManager.Ui.Authorization;

public sealed class PermissionAuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public PermissionAuthorizeAttribute()
    {
        Permissions = Permissions.None;
    }
    
    public PermissionAuthorizeAttribute(Permissions permission)
    {
        Permissions = permission;
    }

    public Permissions Permissions
    {
        get => !string.IsNullOrEmpty(Policy) ? PolicyNameHelper.GetPermissionsFrom(Policy) : Permissions.None;
        set => Policy = value != Permissions.None ? PolicyNameHelper.GeneratePolicyNameFor(value) : string.Empty;
    }
}
