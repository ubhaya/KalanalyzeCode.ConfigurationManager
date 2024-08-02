using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Categories.Queries;

public class GetAppSettingsRequestHandler : IRequestHandler<GetAppSettingsRequest, GetAppSettingsResponse>
{
    private readonly RepositoryService _repository;

    public GetAppSettingsRequestHandler(RepositoryService repository)
    {
        _repository = repository;
    }

    public async Task<GetAppSettingsResponse> Handle(GetAppSettingsRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllApplicationSettings(request.SettingName, cancellationToken);
        return new GetAppSettingsResponse()
        {
            Settings = result
        };
    }
}