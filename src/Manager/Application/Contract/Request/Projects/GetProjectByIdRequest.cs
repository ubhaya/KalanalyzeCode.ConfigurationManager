using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;

public record GetProjectByIdRequest(Guid Id) : IRequest<ResponseDataModel<GetProjectByIdResponse>>;