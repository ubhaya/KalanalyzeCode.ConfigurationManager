namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class ServerSideServiceExtension
{
    public static IServiceCollection AddServerSideServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        return services;
    }
}