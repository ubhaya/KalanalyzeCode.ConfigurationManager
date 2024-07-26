using KalanalyzeCode.ConfigurationManager.Ui;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"https://localhost:7015") });
builder.Services.AddMudServices();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IClient>()
    .AddClasses(@class => @class.AssignableTo<IClient>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

await builder.Build().RunAsync();