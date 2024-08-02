namespace KalanalyzeCode.ConfigurationManager.Application.Helpers;

public class AppConstants
{
    public const string CorsPolicy = nameof(CorsPolicy);
    
    public class LoggingMessages
    {
        public const string LoggingBehaviour = "Minimal API Request: {Name} {@Request}";
        public const string TransactionBehaviour = "Request failed: Rolling back all the changes made to the Context";
        public const string DatabaseSeed = "Database seed to {Table}. Count={Count}";
        public const string DatabaseSaved = "Database saved successfully at {Date} - {Time}";
        public const string TransactionAlreadyCreated = "A transaction with ID {ID} is already created";
        public const string TransactionCreated = "A new transaction was created with ID {ID}";
        public const string TransactionCommited = "Commiting Transaction {ID}";
        public const string TransactionRollingBack = "Rolling back Transaction {ID}";
        public const string NewDomainEvent = "New domain event {Event}";
        public const string SeedingOrMigrationError = "An error occurred while migrating or initializing the database";
    }

    public class Routes
    {
        private const string Api = nameof(Api);
        public const string GetAppSettings = $"{nameof(Api)}/{nameof(GetAppSettings)}";

        public class Projects
        {
            public const string GetAll = $"{nameof(Api)}/{nameof(Projects)}";
        }
    }
}