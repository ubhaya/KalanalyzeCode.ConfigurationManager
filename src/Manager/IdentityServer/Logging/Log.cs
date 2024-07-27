namespace IdentityServer.Logging;

public static partial class Log
{
    [LoggerMessage(LogLevel.Debug, "{Role} created")]
    public static partial void LogRoleCreated(this ILogger logger, string role);
    
    [LoggerMessage(LogLevel.Debug, "{Role} already exists")]
    public static partial void LogRoleAlreadyExists(this ILogger logger, string role);
    
    [LoggerMessage(LogLevel.Debug, "{User} created")]
    public static partial void LogUserCreated(this ILogger logger, string user);
    
    [LoggerMessage(LogLevel.Debug, "{User} already exists")]
    public static partial void LogUserAlreadyExists(this ILogger logger, string user);
    
    [LoggerMessage(LogLevel.Debug, "{User} already in {Role}")]
    public static partial void LogUserAlreadyInRole(this ILogger logger, string user, string role);
    
    [LoggerMessage(LogLevel.Debug, "{User} add to {Role}")]
    public static partial void LogUserAddToRole(this ILogger logger, string user, string role);
}