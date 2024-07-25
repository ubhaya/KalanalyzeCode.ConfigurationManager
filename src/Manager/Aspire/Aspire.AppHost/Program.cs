var builder = DistributedApplication.CreateBuilder(args);

var identityServerService = builder.AddProject<Projects.IdentityServer>("identityserver");
var apiService = builder.AddProject<Projects.Api>("configuration-manager-api")
    .WithReference(identityServerService);

builder.AddProject<Projects.Ui>("configuration-manager-ui")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);


builder.Build().Run();
