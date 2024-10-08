using KalanalyzeCode.ConfigurationManager.Application;
using KalanalyzeCode.ConfigurationManager.Ui.Extension;
using KalanalyzeCode.ConfigurationManager.Ui.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables("ConfigurationManager_");
// Add MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddControllers();

builder.Services.AddSwaggerService();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationCore();

builder.Services.AddPersistence(builder.Configuration);

builder.AddIdentityService();

builder.AddIdentityServerFunction();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOpenIdConnect();

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();
if (args.Contains("/seed"))
{
    await app.SeedData();
}

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
    .AddInteractiveServerRenderMode();

app.UseIdentityServerFunction();

// app.UseOpenIdConnectEndpoint();

app.MapControllers();

await app.RunAsync();