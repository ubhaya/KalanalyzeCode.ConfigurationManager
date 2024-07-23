using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Categories.Queries;

public class GetAppSettingsRequestHandler : IRequestHandler<GetAppSettingsRequest, IResult>
{
    public async Task<IResult> Handle(GetAppSettingsRequest request, CancellationToken cancellationToken)
    {
        var repo = new RepositoryService();
        var result = await repo.GetAllApplicationSettings(request.SettingName);
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