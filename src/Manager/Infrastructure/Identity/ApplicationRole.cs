using KalanalyzeCode.ConfigurationManager.Shared.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KalanalyzeCode.ConfigurationManager.Infrastructure.Identity;

public class ApplicationRole : IdentityRole
{
    public Permissions Permissions { get; set; }
}