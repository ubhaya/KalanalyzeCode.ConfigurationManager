using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Categories.Queries;

public class GetAppSettingsRequestHandler : IRequestHandler<GetAppSettingsRequest, ResponseDataModel<GetAppSettingsResponse>>
{
    private readonly RepositoryService _repository;

    public GetAppSettingsRequestHandler(RepositoryService repository)
    {
        _repository = repository;
    }

    public async Task<ResponseDataModel<GetAppSettingsResponse>> Handle(GetAppSettingsRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllApplicationSettings(request.SettingName, cancellationToken);
        return new ResponseDataModel<GetAppSettingsResponse>
        {
            Data = new GetAppSettingsResponse()
            {
                Settings = result
            },
            Success = result.Count != 0,
        };
    }
}