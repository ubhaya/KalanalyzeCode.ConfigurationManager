using KalanalyzeCode.ConfigurationManager.Entity.Entities;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;

public sealed class GetProjectByIdResponse
{
    public Project? Project { get; set; }
}