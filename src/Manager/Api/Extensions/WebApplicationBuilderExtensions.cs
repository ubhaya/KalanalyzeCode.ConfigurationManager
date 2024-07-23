using KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;
using MediatR;
using Serilog;
using Serilog.Events;

namespace KalanalyzeCode.ConfigurationManager.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
    }

    public static WebApplication MediateGet<TRequest>(
        this WebApplication app,
        string template, string name, params string[] tags) where TRequest : IHttpRequest
    {
        app.MapGet(template,
            async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request))
            .WithName(name)
            .WithTags(tags);
        return app;
    }
}