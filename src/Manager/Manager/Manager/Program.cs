using KalanalyzeCode.ConfigurationManager.Ui.Client.Extension;
using KalanalyzeCode.ConfigurationManager.Ui.Extension;
using KalanalyzeCode.ConfigurationManager.Ui.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Manager.Components.Account;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddControllers();

builder.Services.AddSwaggerService();

builder.AddDatabaseConnect();

builder.AddIdentityService();

builder.AddIdentityServerFunction();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddOpenIdConnect();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();

builder.Services.AddServerSideServices();

var app = builder.Build();

await app.SeedData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapSwagger();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(KalanalyzeCode.ConfigurationManager.Ui.Client._Imports).Assembly);

app.UseIdentityServerFunction();

// app.UseOpenIdConnectEndpoint();

app.MapControllers();

await app.RunAsync();