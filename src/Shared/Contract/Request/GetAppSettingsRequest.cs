using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Response;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;

public class GetAppSettingsRequest : IRequest<ResponseDataModel<GetAppSettingsResponse>>
{
    public string SettingName { get; set; } = string.Empty;
}