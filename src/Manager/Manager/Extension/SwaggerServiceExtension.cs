namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class SwaggerServiceExtension
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
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

        return app;
    }
}