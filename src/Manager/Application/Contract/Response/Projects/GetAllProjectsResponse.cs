using KalanalyzeCode.ConfigurationManager.Entity.Entities;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;

public sealed record GetAllProjectsResponse
{
    public IEnumerable<Project> Projects { get; set; } = [];
    public int TotalItem { get; set; }
}