using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;

public sealed class CreateProjectRequest : IRequest<CreateProjectResponse>
{
    public string ProjectName { get; set; } = string.Empty;
}