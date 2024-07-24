using KalanalyzeCode.ConfigurationManager.Api;
using KalanalyzeCode.ConfigurationManager.Api.Extensions;
using KalanalyzeCode.ConfigurationManager.Application;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Aspire.ServiceDefaults;
using KalanalyzeCode.ConfigurationManager.Shared;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Response;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Host.AddSerilog();
builder.Services.AddWebApiConfig();
builder.Services.AddApplicationCore();
builder.Services.AddPersistence(builder.Configuration);

var app = builder.Build();

await app.SeedDatabase();

app.UseCors(AppConstants.CorsPolicy);
app.UseStaticFiles();
app.MapSwagger();


app.MediateGet<GetAppSettingsRequest>(ProjectConstant.GetAppSettings, nameof(GetAppSettingsRequest), nameof(GetAppSettingsResponse));

await app.RunAsync();