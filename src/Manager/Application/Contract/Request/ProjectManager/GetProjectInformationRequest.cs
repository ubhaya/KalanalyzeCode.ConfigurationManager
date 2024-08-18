using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using LanguageExt;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ProjectManager;

public sealed record GetProjectInformationRequest(Guid Id) : IRequest<Option<Project>>;