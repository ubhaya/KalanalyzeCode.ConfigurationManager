using FluentValidation;
using KalanalyzeCode.ConfigurationManager.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Application;

public static class DependencyConfig
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Application));
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Application).Assembly);
            config.AddOpenBehavior(typeof(TransactionBehaviour<,>));
        });
        services.AddValidatorsFromAssemblyContaining(typeof(Application));

        services.AddScoped<RepositoryService>();

        return services;
    }
}