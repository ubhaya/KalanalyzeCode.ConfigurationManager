﻿using IdentityServer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");
foreach (var client in Config.Clients)
{
    foreach (var redirectUri in client.RedirectUris)
    {
        Log.Information("{Client} has {RedirectUri}", client.ClientName, redirectUri);
    }
}
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddEnvironmentVariables("IdentityServer_");
    
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName)
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    // this seeding is only for the template to bootstrap the DB and users.
    // in production you will likely want a different approach.
    // if (args.Contains("/seed"))
    // {
        Log.Information("Seeding database...");
        await SeedData.EnsureSeedData(app);
        Log.Information("Done seeding database. Exiting.");
        // return;
    // }
    
    await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}