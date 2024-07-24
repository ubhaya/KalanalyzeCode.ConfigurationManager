using KalanalyzeCode.ConfigurationManager.Domain.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Categories.Queries;

public class GetAppSettingsRequestHandler : IRequestHandler<GetAppSettingsRequest, IResult>
{
    private readonly RepositoryService _repository;

    public GetAppSettingsRequestHandler(RepositoryService repository)
    {
        _repository = repository;
    }

    public async Task<IResult> Handle(GetAppSettingsRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllApplicationSettings(request.SettingName, cancellationToken);
        return Results.Ok(new ResponseDataModel<GetAppSettingsResponse>
        {
            Data = new GetAppSettingsResponse()
            {
                Settings = result
            },
            Success = result.Count != 0,
        });
    }
}