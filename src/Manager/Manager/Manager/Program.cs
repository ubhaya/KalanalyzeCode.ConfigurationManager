using KalanalyzeCode.ConfigurationManager.Ui;
using MudBlazor.Services;
using KalanalyzeCode.ConfigurationManager.Ui.Components;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

builder.AddIdentityServerFunction();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(KalanalyzeCode.ConfigurationManager.Ui.Client._Imports).Assembly);

app.UseIdentityServerFunction();

app.Run();

public static class Extension
{
    public static WebApplicationBuilder AddIdentityServerFunction(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddTestUsers(TestUsers.Users);

        return builder;
    }

    public static WebApplication UseIdentityServerFunction(this WebApplication app)
    {
        app.UseStaticFiles();
        app.UseRouting();
        
        app.UseIdentityServer();
        
        app.UseAuthorization();
        app.UseAntiforgery();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}