using System.Collections.ObjectModel;

namespace KalanalyzeCode.ConfigurationManager.Application.Helpers;

public class AppConstants
{
    public const string CorsPolicy = nameof(CorsPolicy);
    
    public class LoggingMessages
    {
        public const string LoggingBehaviour = "Minimal API Request: {Name} {@Request}";
        public const string TransactionBehaviour = "Request failed: Rolling back all the changes made to the Context";
        public const string DatabaseSeed = "Database seed. Count={Count}";
        public const string DatabaseSaved = "Database saved successfully at {Date} - {Time}";
        public const string TransactionAlreadyCreated = "A transaction with ID {ID} is already created";
        public const string TransactionCreated = "A new transaction was created with ID {ID}";
        public const string TransactionCommited = "Commiting Transaction {ID}";
        public const string TransactionRollingBack = "Rolling back Transaction {ID}";
        public const string NewDomainEvent = "New domain event {Event}";
        public const string SeedingOrMigrationError = "An error occurred while migrating or initializing the database";
    }
    
    public class Identity
    {
        public const string ScopeName = "clean_architecture_maui_api";
        public const string DisplayName = "CleanArchitecture.Maui.Api";
        public const string ClientId = "CleanArchitecture.Maui.MobileUi";
        public const string ClientName = "Clean Architecture Maui app";
        public static readonly List<string> AllowedScope = ["clean_architecture_maui_api"];
    }
}