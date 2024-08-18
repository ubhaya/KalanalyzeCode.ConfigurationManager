using KalanalyzeCode.ConfigurationManager.Entity.Entities;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response.ProjectManager;

public class GetProjectInformationResponse
{
    public Project Project { get; set; } = default!;
}