# Getting Started

## Prerequisites
Before you begin, ensure you have the following installed on your system:
- **Docker**: To run the project in a containerized environment.
- **Docker Compose**: To set up and manage multi-container applications.

## Installation and Setup

1. **Create a Docker Compose File**
   Since the project requires a PostgreSQL database, you’ll need a Docker Compose file to set up the necessary services. Create a `docker-compose.yml` file in the project directory with the following content:

   ```yaml
   version: '3.8'

   services:
     postgresql:
       image: %postgresqlDockerRepo%
       environment:
         POSTGRES_USER: your_username
         POSTGRES_PASSWORD: your_password
       networks:
         configuration-manager

     configManager:
       image: %managerDockerRepo%
       depends_on:
         - postgresql
       environment:
         - ConfigurationManager_PostgreSql__ConnectionString=Host=postgresql;Username=your_username;Database=myDatabase
         - ConfigurationManager_PostgreSql__DbPassword=your_password
         - ASPNETCORE_Kestrel__Certificates__Default__Password=KalanalyzeCode.ConfigurationManager@753951
         - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/kalanalyzecode.configurationmanager.pfx
       ports:
         - 7206:7206
         - 5225:5225
       networks:
         configuration-manager
   
   networks:
     configuration-manager:
     driver: bridge
   ```

2. **Start the Services**
   Run the following command to start the PostgreSQL database and the Configuration Manager service:

   ```bash
   docker-compose up -d
   ```

   This will spin up the PostgreSQL database and the Configuration Manager service. The database migration will be handled automatically by the Configuration Manager when it starts.

4. **Accessing the Configuration Manager**
   Once the containers are running, the Configuration Manager will be accessible at `https://localhost:7206` `http://localhost:5225`.

5. **Install the Provider in Your Project**
   To use the Configuration Manager in your .NET project, install the corresponding NuGet package:

   ```bash
   dotnet add package %providerNugetPackage%
   ```

6. **Configure Your Application**
   In your .NET application, configure the provider to retrieve settings from the Configuration Manager. Here’s a basic example of how to do this in your `Program.cs`:

   ```csharp
   var builder = WebApplication.CreateBuilder(args);
   
   builder.Configuration.AddConfigurationManager(options =>
   {
        options.SecreteManagerOptions = new()
        {
             BaseAddress = new Uri("https://localhost:7206"),
             Scopes = 
                     [
                         "KalanalyzeCode.ConfigurationManager", 
                         "profile", 
                         "openid"
                     ],
             ClientId = "postman.apikey",
             ClientSecrete = "secret",
             ApiKey = "f05285ec-838c-444d-9323-e56aced7a7cb"
        };
   
        options.ReloadPeriodically = true;
        options.PeriodInSeconds = 2;
   });

   builder.Services.Configure<StarfishOptions>
       (builder.Configuration.GetSection(nameof(StarfishOptions)));
   ```