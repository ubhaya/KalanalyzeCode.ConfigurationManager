using KalanalyzeCode.ConfigurationManager.Provider;
using KalanalyzeCode.ConfigurationManager.Sample.Provider;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddConfigurationManager(options =>
{
    options.SecreteManagerOptions = new()
    {
        BaseAddress = new Uri("https://localhost:7015"),
        Scopes = ["KalanalyzeCode.ConfigurationManager", "profile", "openid"],
        ClientId = "postman.apikey",
        ClientSecrete = "secret",
        ApiKey = "674e2a077bd64d669340af69c460767c"
    };
        
    options.ReloadPeriodically = true;
    options.PeriodInSeconds = 2;
});

builder.Services.Configure<StarfishOptions>(builder.Configuration.GetSection(nameof(StarfishOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/optionmonitor", 
        (IOptionsMonitor<StarfishOptions> optionMonitor) => optionMonitor.CurrentValue)
    .WithName("OptionMonitor")
    .WithOpenApi();

app.Run();

namespace KalanalyzeCode.ConfigurationManager.Sample
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}