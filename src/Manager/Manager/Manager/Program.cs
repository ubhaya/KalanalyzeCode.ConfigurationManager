using KalanalyzeCode.ConfigurationManager.Ui.Extension;
using KalanalyzeCode.ConfigurationManager.Ui.Components;
using MudBlazor.Services;

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
    .AddInteractiveServerComponents();

builder.Services.AddOpenIdConnect();

builder.Services.AddCascadingAuthenticationState();

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
    .AddInteractiveServerRenderMode();

app.UseIdentityServerFunction();

// app.UseOpenIdConnectEndpoint();

app.MapControllers();

await app.RunAsync();