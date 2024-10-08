﻿using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request;

public class GetAppSettingsRequest : IRequest<GetAppSettingsResponse>
{
    public string ProjectName { get; set; } = string.Empty;
}