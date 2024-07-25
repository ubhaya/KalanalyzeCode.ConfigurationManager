using KalanalyzeCode.ConfigurationManager.Application;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;

namespace KalanalyzeCode.ConfigurationManager.Api;

public static class DependencyConfig
{
    public static IServiceCollection AddWebApiConfig(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: AppConstants.CorsPolicy,
                builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddOpenApiDocument(c =>
        {
            c.Title = "Minimal APIs";
            c.Version = "v1";
        });


        return services;
    }

    public static WebApplication MapSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // app.UseOpenApi();
        // app.UseSwaggerUi(settings => { settings.Path = "/api"; });

        return app;
    }
}