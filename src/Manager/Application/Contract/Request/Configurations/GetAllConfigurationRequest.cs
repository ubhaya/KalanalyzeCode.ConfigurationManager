using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;

public sealed record GetAllConfigurationRequest(
    string SearchString,
    int Page,
    int PageSize,
    CustomSortDirection SortDirection,
    string SortLabel,
    Guid ProjectId) : IRequest<GetAllConfigurationResponse>;