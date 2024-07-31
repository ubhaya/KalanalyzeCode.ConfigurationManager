using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Queries;

public class GetAllProjectsRequestHandler : IRequestHandler<GetAllProjectsRequest, ResponseDataModel<GetAllProjectsResponse>>
{
    public Task<ResponseDataModel<GetAllProjectsResponse>> Handle(GetAllProjectsRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ResponseDataModel<GetAllProjectsResponse>());
    }
}