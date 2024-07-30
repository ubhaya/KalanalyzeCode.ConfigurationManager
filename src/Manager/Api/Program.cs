using KalanalyzeCode.ConfigurationManager.Api;
using KalanalyzeCode.ConfigurationManager.Api.Endpoints;
using KalanalyzeCode.ConfigurationManager.Api.Extensions;
using KalanalyzeCode.ConfigurationManager.Application;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();
builder.Services.AddWebApiConfig(builder.Configuration);
builder.Services.AddApplicationCore();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddEndpointDefinitions(typeof(IEndpointsDefinition));

var app = builder.Build();

await app.SeedDatabase();

app.UseCors(AppConstants.CorsPolicy);
app.UseStaticFiles();
app.MapSwagger();

app.UseEndpointDefinition();

await app.RunAsync();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
    
