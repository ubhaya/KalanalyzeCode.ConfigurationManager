using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KalanalyzeCode.ConfigurationManager.Application.Common.Models;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public Permissions Permissions { get; set; }
}