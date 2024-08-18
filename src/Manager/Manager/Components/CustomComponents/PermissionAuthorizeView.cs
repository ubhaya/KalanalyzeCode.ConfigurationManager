using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KalanalyzeCode.ConfigurationManager.Ui.Components.CustomComponents;

public class PermissionAuthorizeView : AuthorizeView
{
    [Parameter, EditorRequired]
#pragma warning disable BL0007
    public Permissions Permissions
#pragma warning restore BL0007
    {
        get
        {
            return string.IsNullOrEmpty(Policy) ? 
                Permissions.None : 
                PolicyNameHelper.GetPermissionsFrom(Policy);
        }
        set
        {
            Policy = PolicyNameHelper.GeneratePolicyNameFor(value);
        }
    }
}