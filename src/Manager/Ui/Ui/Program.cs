using KalanalyzeCode.ConfigurationManager.Shared;
using MudBlazor.Services;
using KalanalyzeCode.ConfigurationManager.Ui;
using KalanalyzeCode.ConfigurationManager.Ui.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient(ProjectConstant.ConfigurationManagerClient,
    client => client.BaseAddress = new($"https+http://{ProjectConstant.ConfigurationManagerApi}"));

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient(ProjectConstant.ConfigurationManagerClient));

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IClient>()
    .AddClasses(@class => @class.AssignableTo<IClient>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();