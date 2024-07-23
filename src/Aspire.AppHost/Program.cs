var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Manager>("configurationmanager");

builder.Build().Run();
