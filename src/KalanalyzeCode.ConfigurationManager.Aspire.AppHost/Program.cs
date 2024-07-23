var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.KalanalyzeCode_ConfigurationManager>("configurationmanager");

builder.Build().Run();
