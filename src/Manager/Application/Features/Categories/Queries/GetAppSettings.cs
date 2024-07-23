using AutoMapper;
using Carter;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Categories.Queries;

public class GetAppSettings : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/appsettings",
                async (string settingName, IMediator mediator, CancellationToken cancellationToken = default) =>
                {
                    var result = await mediator.Send(new Query(settingName), cancellationToken);
                    return Results.Ok(result);
                })
            .WithName(nameof(GetAppSettings))
            .WithTags(nameof(ConfigurationSettings));
    }

    public record Query(string SettingName) : IRequest<ResponseDataModel<IEnumerable<ApplicationSettings>>>;
    
    public class QueryHandler : IRequestHandler<Query,ResponseDataModel<IEnumerable<ApplicationSettings>>>
    {
        public async Task<ResponseDataModel<IEnumerable<ApplicationSettings>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var repo = new RepositoryService();
            var result = await repo.GetAllApplicationSettings(request.SettingName);
            return new ResponseDataModel<IEnumerable<ApplicationSettings>>
            {
                Data = result,
                Success = result.Any(),
            };
        }
    }

    public class ApplicationSettingsMappingProfile : Profile
    {
        public ApplicationSettingsMappingProfile() => CreateMap<ConfigurationSettings, ApplicationSettings>();
    }
}