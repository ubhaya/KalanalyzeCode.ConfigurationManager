using KalanalyzeCode.ConfigurationManager.Ui.Client.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KalanalyzeCode.ConfigurationManager.Ui.Models;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public Permissions Permissions { get; set; }
}