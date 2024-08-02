using KalanalyzeCode.ConfigurationManager.Entity.Entities;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;

public sealed class CreateProjectResponse
{
    public Project? Project { get; set; }
}