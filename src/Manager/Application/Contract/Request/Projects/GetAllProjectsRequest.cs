using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;

public sealed record GetAllProjectsRequest(
    string? SearchString,
    int Page = 0,
    int PageSize = 0,
    CustomSortDirection SortDirection = CustomSortDirection.None,
    string SortColumnName = "") : IRequest<ResponseDataModel<GetAllProjectsResponse>>;