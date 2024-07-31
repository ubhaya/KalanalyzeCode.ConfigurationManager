namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;

public sealed record GetAllProjectsResponse
{
    public string Name { get; set; } = string.Empty;
}