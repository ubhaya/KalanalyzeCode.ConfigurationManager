var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Api>("configurationmanager");

builder.Build().Run();
