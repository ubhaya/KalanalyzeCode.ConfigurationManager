var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Api>("configuration-manager-api");

builder.AddProject<Projects.Ui>("configuration-manager-ui")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
