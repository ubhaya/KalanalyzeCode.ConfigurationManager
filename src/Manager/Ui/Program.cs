using KalanalyzeCode.ConfigurationManager.Ui;
using MudBlazor.Services;
using KalanalyzeCode.ConfigurationManager.Ui.Components;
using KalanalyzeCode.ConfigurationManager.Ui.Extensions;
using KalanalyzeCode.ConfigurationManager.Ui.HttpHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOpenIdConnect();

builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri($"https://localhost:7015"))
    .AddHttpMessageHandler<AuthenticationDelegatingHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));

builder.Services.AddHttpContextAccessor();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/account/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
});

app.MapGet("/account/login", async (string redirectUri, HttpContext context) =>
{
    await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = redirectUri });
});

app.Run();